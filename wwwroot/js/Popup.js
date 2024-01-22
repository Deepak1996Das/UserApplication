//var timerInterval=setInterval(() => {
//    TimerFunction();
//}, 10);


//const TimerFunction = () => {

//    var startTime = new Date(document.getElementById('popupSessionState').value);
//    let currentTime = new Date();
//    let timeDifference = currentTime - startTime;
//    console.log("timeDifference", timeDifference);
//    if (timeDifference > 20000) {
//        fn();
//    }

//}

//const fn = () => {
//    var over= document.getElementById('overlay')
//    if (over != null) {
//        over.style.display = 'block';
//        document.getElementById('popup').style.display = 'block';
//    }
//    else {
//        clearInterval(timerInterval)
//    }
//}







setTimeout(() => {
    console.log("Deepak");
    document.getElementById('overlay').style.display = 'block';
    document.getElementById('popup').style.display = 'block';
},20000)