function check()
{
    x = document.getElementById("username").value;
    y  = document.getElementById("mpassword").value;
    if (x == "") {
        alert("خو username رو وارد کن");
            return false;
    }
    if (y == "")
    {
        alert("زرنگی؟؟؟ بدون رمز میخوای وارد شی؟");
        return false;
    }
    return true;
}