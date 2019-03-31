var global_gameInstance = null;
function UnityProgress(gameInstance, progress) {
	//console.log(gameInstance);
	if (global_gameInstance == null){
		global_gameInstance = gameInstance;
	}
	
  if (!gameInstance.Module)
    return;
  /*if (!gameInstance.logo) {
    gameInstance.logo = document.createElement("div");
    gameInstance.logo.className = "logo " + gameInstance.Module.splashScreenStyle;
    gameInstance.container.appendChild(gameInstance.logo);
  }*/
  if (!gameInstance.progress) {
	  var tpl_el = document.getElementsByClassName('progress')[0];
	  var tpl = tpl_el.innerHTML;
    gameInstance.progress = document.createElement("div");
	gameInstance.progress.innerHTML = tpl;
	
    /*gameInstance.progress.className = "progress " + gameInstance.Module.splashScreenStyle;
    gameInstance.progress.empty = document.createElement("div");
    gameInstance.progress.empty.className = "empty";
    gameInstance.progress.appendChild(gameInstance.progress.empty);*/
    gameInstance.progress.full = document.getElementsByClassName('progres')[0];
	
	/*
    gameInstance.progress.full.className = "full";
    gameInstance.progress.appendChild(gameInstance.progress.full);*/
    gameInstance.container.appendChild(gameInstance.progress);
  }
  gameInstance.progress.full.style.width = (100 * progress) + "%";
  //gameInstance.progress.empty.style.width = (100 * (1 - progress)) + "%";
  if (progress == 1)
  {
	  /*gameInstance.logo.style.display = *///gameInstance.progress.style.display = "none";
  }
    
}
function hideprogress(){
	var gameInstance = global_gameInstance;
	gameInstance.progress.style.display = "none";
}
function MyInitializationCallbackFunction(var1,var2,var3){
	console.log('MyInitializationCallbackFunction');
	console.log(var1);
	console.log(var2);
	console.log(var3);
}