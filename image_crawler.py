import os
import requests
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import TimeoutException
from bs4 import BeautifulSoup
from PIL import Image
from io import BytesIO
import urllib3
from check_images import save_finishDn_images
from dbPics import dbPics

# 搜尋並下載圖片
def imageSearch(keyword, counts, finishDn_images, finishDn_file, folder_path, dbPics, result):
    
    if not os.path.exists(folder_path):
        os.makedirs(folder_path)

    chrome_option = webdriver.ChromeOptions()
    chrome_option.add_experimental_option("excludeSwitches", ["enable-automation"])
    chrome_option.add_argument("headless")
    chrome_option.add_argument("--disable-dev-shm-usage")
    driver = webdriver.Chrome(options=chrome_option)

    urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
    header = {"user-agent": "Mozilla/5.0"}

    try:
        driver.get(f"https://www.google.com/search?q={keyword}&tbm=isch")
        driver.maximize_window()
        body = driver.find_element(By.TAG_NAME, "body")

        while len(driver.find_elements(By.CSS_SELECTOR, ".mNsIhb")) < counts:
            body.send_keys(Keys.END)
            try:
                WebDriverWait(driver, 2).until(EC.presence_of_element_located((By.TAG_NAME, "body")))
            except TimeoutException:
                break

        body.send_keys(Keys.HOME)
        imageXPathNum = 1
        downloaded_images = 0

        while downloaded_images < counts:
            try:
                driver.find_element(By.XPATH, f"//div[@jsname='dTDiAc'][{imageXPathNum}]/div/h3/a/div/div/div/g-img").click()
                WebDriverWait(driver, 10).until(EC.presence_of_element_located((By.CSS_SELECTOR, ".FyHeAf.iPVvYb")))
                soup = BeautifulSoup(driver.page_source, "html.parser")
                image_tag = soup.select_one(".FyHeAf.iPVvYb")
                title_tag = soup.select_one(".BIB1wf .indIKd.q23Yce.fA1vYb.cS4Vcb-pGL6qe-fwJd0c")

                image_url = image_tag["src"].strip()
                response = requests.get(image_url, headers=header, verify=False, timeout=10)

                if response.status_code == 200:
                    try:
                        img = Image.open(BytesIO(response.content))
                        if (image_url, img.width, img.height) not in finishDn_images:
                            # 確保先儲存圖片，再儲存資料至資料庫
                            # image_name = f"{keyword}_{downloaded_images + 1}.jpg"
                            image_name = f"{keyword}_{downloaded_images + 1}.webp"
                            image_path = os.path.join(folder_path, image_name)
                            img.convert("RGB").save(image_path, format("webp"))
                            # img.save(image_path) # 儲存圖片
                            
                            # 如果圖片儲存成功，才執行資料庫操作
                            db_id = dbPics(title_tag.text.split("-")[0], img.width, img.height, keyword)
                            
                            # 更新檔名以使用資料庫生成的id
                            # new_image_name = f"{db_id}.jpg"
                            new_image_name = f"{db_id}.webp"
                            new_image_path = os.path.join(folder_path, new_image_name)
                            
                            # 修改檔名為資料庫 id 命名格式
                            os.rename(image_path, new_image_path)
            
                            finishDn_images.add((image_url, img.width, img.height))
                            save_finishDn_images(finishDn_images, finishDn_file)
                            
                            result.append({
                                "title": title_tag.text.split("-")[0],
                                "url": image_url,
                                "width": img.width,
                                "height": img.height,
                                "theme": keyword,
                                "path": new_image_path
                            })
                            # dbPics(title_tag.text.split("-")[0], img.width, img.height, keyword)
                            downloaded_images += 1
                    except TimeoutException:  # 畫質太高、沒有高畫質照片，需要太長時間下載
                        imageXPathNum += 1    # 試載下一張照片
                        continue
                    except Exception as e:
                        print(f"Error: {e}")
                    finally:
                        imageXPathNum += 1
            except Exception as e:
                print(f"Error: {e}")
                imageXPathNum += 1
    finally:
        driver.quit()