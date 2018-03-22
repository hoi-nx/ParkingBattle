var path : Array;
var rayColor : Color=Color.white;

function OnDrawGizmos(){
	Gizmos.color=rayColor;
	var path_objs : Array=transform.GetComponentsInChildren(Transform);
	for(var path_obj : Transform in path_objs){
		if(path_obj!=transform)
		path [path.length]=path_obj;

	}
	for(var i: int=0;i<path.length;i++){
		var pos:Vector3=path[i].position;
		if(i>0)
		var prev=path[i-1].position;
		Gizmos.Drawline(prev,pos);
		Gizmos.DrawWireSphere(pos,5);
	}
}