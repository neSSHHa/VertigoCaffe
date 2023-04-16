function showItemInfo
    (
        id,name,description,price,price2,price3,price4,morePrices,image,amount,parentId,parentName,typeName,created_at,updated_at
    )
{
  
    document.getElementById("itemModelTitle").innerText = name + " #" + id;
    document.getElementById("itemModelDescription").innerText = description;
    document.getElementById("itemModelPrice").innerText = price;
    document.getElementById("itemModelPrice2").innerText = price2;
    document.getElementById("itemModelPrice3").innerText = price3;
    document.getElementById("itemModelPrice4").innerText = price4;
    document.getElementById("itemModelMorePrices").innerText = morePrices;
    document.getElementById("itemModelCardId").innerText = parentId;
    document.getElementById("itemModelAmount").innerText = amount;
    document.getElementById("itemModelTypeName").innerText = typeName;  
    document.getElementById("itemImages").setAttribute("src", image);
    document.getElementById("itemModelparentName").innerText = parentName;
    document.getElementById("itemLastUpdatedAt").innerText = updated_at;
    document.getElementById("itemCreatedAt").innerText = created_at;
    document.getElementById("showItems").click();

}
//showUserInfo('@(item.Id)', '@item.UserName', '@item.Email', '@item.PhoneNumber', '@((await _userMenager.GetRolesAsync(item)).FirstOrDefault())', '@item.Image.Replace("\\", "/")');
function showUserInfo
    (
        id, userName, Email, PhoneNumber, role, image
    ) {

    document.getElementById("userName").innerText = userName;
    document.getElementById("UserEmail").innerText = Email;
    document.getElementById("PhoneNumber").innerText = PhoneNumber;
    document.getElementById("UserRole").innerText = role;
    document.getElementById("UserImage").setAttribute("src", image);
    document.getElementById("showUser").click();

}

function validateDelete(name,id) {
    Swal.fire({
        title: 'Are you sure you want to delete ' + name+'?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33', 
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Your item has been deleted',
                showConfirmButton: false,
                timer: 1000
            })
            var url = '/Customer/Home/Delete/' + id;
            window.location.replace(url);
            return true;
        } else {
            return false;
        }
    })
      
}

function validateDeleteUser(name, id) {
    Swal.fire({
        title: 'Are you sure you want to delete ' + name + '?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Your item has been deleted',
                showConfirmButton: false,
                timer: 1000
            })
            var url = '/Customer/Home/DeleteUser/' + id;
            window.location.replace(url);
            return true;
        } else {
            return false;
        }
    })

}
function populateUpdateId(x) {
    //Swal.fire({
    //    position: 'top-end',
    //    icon: 'success',
    //    title: 'Your work has been saved',
    //    showConfirmButton: false,
    //    timer: 1500
    //})
    document.getElementById("EditedCardId").value = x;
    return true;
}
function populateUpdateUserId(x) {
    var password = document.getElementById("userChangePassword").value;
    if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-+_!@#$%^&*.,?]).{8,}$/.test(password) && password != "") {    
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please use stronger password!'
            })
            return false;
    }
    document.getElementById("EditedUserIndex").value = x;
    document.getElementById("userChangePasswordValue").value = password;
    return true;
}
function checkImageCard() {

    var image = document.getElementById("fileImageCreateCard");
    if (image.value == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please include photo!'
        })
        return false;
    }
    return true;
}

function checkImageUser() {

    var image = document.getElementById("fileImageCreateUser");
    if (image.value == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please include photo!'
        })
        return false;
    }
 
    if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-+_!@#$%^&*.,?]).{8,}$/.test(document.getElementById("newUserPassword").value)) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please use strong password!'
        })
        return false;
    }
    if (document.getElementById("newUserName").value == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please Include Name'
        })
        return false;
    }
    if (document.getElementById("newUserPhone").value == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please Include Phone Number'
        })
        return false;
    }
    if (document.getElementById("newUserEmail").value == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please Include Email Adress'
        })
        return false;
    }
    return true;
}
