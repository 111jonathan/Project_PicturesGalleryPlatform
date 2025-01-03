        document.getElementById('name-125c').addEventListener('input', function () {
            const pattern = /^[\w\u4e00-\u9fa5]{2,12}$/;
            if (!pattern.test(this.value)) {
                this.setCustomValidity("Account Format Error, it should between 2-12 Characters");
            } else {
                this.setCustomValidity("");
            }
        }); 

        // �Ą�: ���� Name ʹ�� RE �z������ 2~12 ����Ԫ
        document.getElementById('name-9558').addEventListener('input', function () {
            const pattern = /^[\w\u4e00-\u9fa5]{2,22}$/;
            if (!pattern.test(this.value)) {
                this.setCustomValidity("Name Format Error, it should between 2-22 Characters");
            } else {
                this.setCustomValidity("");
            }
        });

        // �Ą�: ���� Email ʹ�� RE �z����M�㳣�ø�ʽ
        document.getElementById('email-6797').addEventListener('input', function () {
            const pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(\.[a-zA-Z]{2,})?$/;
            if (!pattern.test(this.value)) {
                this.setCustomValidity("Email Format Not Correct, please modify");
            } else {
                this.setCustomValidity("");
            }
        });

        // �Ą�: �ܴa���� 5~12 ����Ԫ
        document.getElementById('text-ef9e').addEventListener('input', function () {
            const pattern = /^[\w!@@#$%^&*]{5,12}$/;
            if (!pattern.test(this.value)) {
                this.setCustomValidity("Password Format Error, it should between 5-12 Characters");
            } else {
                this.setCustomValidity("");
            }
        });
