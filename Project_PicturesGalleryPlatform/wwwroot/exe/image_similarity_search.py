import os
import numpy as np
from tensorflow.keras.applications.vgg16 import VGG16, preprocess_input
from tensorflow.keras.preprocessing import image
from sklearn.metrics.pairwise import cosine_similarity
from concurrent.futures import ThreadPoolExecutor
import sys

# 設定TensorFlow的日誌級別，將非錯誤日誌隱藏
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'

# 載入VGG16模型（不包括頂層分類器）
model = VGG16(weights='imagenet', include_top=False, input_shape=(224, 224, 3))

# 解析命令行參數並設置特徵緩存路徑
feature_cache_folder = sys.argv[3]
if not os.path.exists(feature_cache_folder):
    os.makedirs(feature_cache_folder)

# 提取圖片特徵並L2正規化，並儲存至本地
def extract_features(img_path):
    cache_path = os.path.join(feature_cache_folder, f"{os.path.basename(img_path)}.npy")
    
    # 如果特徵已經存在，直接加載
    if os.path.exists(cache_path):
        return np.load(cache_path)

    # 否則提取特徵並保存
    img = image.load_img(img_path, target_size=(224, 224))
    img_array = np.expand_dims(image.img_to_array(img), axis=0)
    img_array = preprocess_input(img_array)
    features = model.predict(img_array).flatten()
    normalized_features = features / np.linalg.norm(features)  # L2正規化

    np.save(cache_path, normalized_features)  # 保存特徵
    return normalized_features

# 圖片路徑
image_folder = sys.argv[2]
image_paths = [os.path.join(image_folder, img) for img in os.listdir(image_folder) if img.lower().endswith(('.png', '.jpg', '.jpeg'))]

# 使用多執行緒加速特徵提取
with ThreadPoolExecutor(max_workers=16) as executor:
    features_list = list(executor.map(extract_features, image_paths))

# 查詢圖片特徵
query_image_path = sys.argv[1]
query_features = extract_features(query_image_path)

# 計算相似度
similarities = cosine_similarity([query_features], features_list)[0]

# 排序並篩選出相似度高於給定閾值的圖片
top_indices = np.argsort(similarities)[::-1]

# 根據命令行參數設置相似度閾值，默認為0.1
similarity_threshold = eval(sys.argv[4]) if len(sys.argv) == 5 else 0.1

# 過濾相似度大於閾值的圖片
filtered_image_names = [os.path.splitext(os.path.basename(image_paths[idx]))[0] for idx in top_indices if similarities[idx] > similarity_threshold]

# 輸出過濾後的圖片名稱
print(" ".join(filtered_image_names))
