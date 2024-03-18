function updateImagePreview(selector) {
    const inputElement = document.getElementById(selector);
    if (inputElement.files && inputElement.files[0]) {
        const file = inputElement.files[0];
        const previewUrl = URL.createObjectURL(file);

        //
        document.getElementById('ClientPicLogout').src = previewUrl;
        document.getElementById('ClientPic').src = previewUrl;
       
    }
}

(async () => {

    try {
        const response = await fetch(`/MBMemberInfo/MemProfileForPicture`);
        if (!response.ok) {
            throw new Error(`${response.status}`);
            return;
        }

        const blob = await response.blob();
        if (blob.type != "image/jpeg") {
            return;
        }
        const imgUrl = URL.createObjectURL(blob);
        document.getElementById('ClientPicLogout').src = imgUrl;
        document.getElementById('ClientPic').src = imgUrl;
    }
    catch (error) {
        console.log(error);
    }

})();