#pragma strict
var car : Transform;
var wheelFL : WheelCollider;
var wheelRL : WheelCollider;
var wheelFR : WheelCollider;
var wheelRR : WheelCollider;
var wheelFLTrans :Transform;
var wheelFRTrans :Transform;
var wheelRLTrans :Transform;
var wheelRRTrans :Transform;
var lowestSteerAtSpeed : float=50;
var lowSpeedSteerAngel : float=15;
var highSpeedSteerAngel : float=5;
var decelerationSpeed : float=25;
var maxTorque : float=1;
var currentSpeed : float;
var topSpeed : float=190;
var maxReverseSpeed : float=50;
var throttle : float=0;
var braked : boolean=false;
var maxBrakeTorque:float=100;
private var mySidewayFriction : float;
private var myForwardFriction : float;
private var slipSidewayFriction : float;
private var slipForwardFriction : float;
public var brakeLights : Material;
public var carObj : GameObject;

function Start(){
	GetComponent.<Rigidbody>().centerOfMass.x=0;
	GetComponent.<Rigidbody>().centerOfMass.y=0;
	GetComponent.<Rigidbody>().centerOfMass.z=0;
	SetValues();
}
function SetValues(){
//ma sat truowjt
	myForwardFriction=wheelRR.forwardFriction.stiffness;
	//ma sats ngang
	mySidewayFriction=wheelRR.sidewaysFriction.stiffness;
	slipForwardFriction=0.04;
	slipSidewayFriction=0.25;
}
function FixedUpdate(){
	Control();
	HandBrake();
}
function Update(){
     if(Input.GetKey(KeyCode.Escape)){
	 	 Application.LoadLevel(0);
	 }

	 if(Input.GetKeyDown(KeyCode.F)){
	 	 carObj.active=false;
	}

	 wheelFLTrans.Rotate(wheelFL.rpm/60*360*Time.deltaTime,0,0);
	 wheelFRTrans.Rotate(wheelFR.rpm/60*360*Time.deltaTime,0,0);
	 wheelRLTrans.Rotate(wheelRL.rpm/60*360*Time.deltaTime,0,0);
	 wheelRRTrans.Rotate(wheelRR.rpm/60*360*Time.deltaTime,0,0);

	 wheelFLTrans.localEulerAngles.y=wheelFL.steerAngle-wheelFLTrans.localEulerAngles.z;
	 wheelFRTrans.localEulerAngles.y=wheelFR.steerAngle-wheelFRTrans.localEulerAngles.z;


	 throttle=Input.GetAxis("Vertical");
	 if(throttle<0.0)
	 brakeLights.SetFloat("_Intensity",Mathf.Abs(throttle));
	 else if(braked)
	  brakeLights.SetFloat("_Intensity",Mathf.Abs(throttle));
	  else
	   brakeLights.SetFloat("_Intensity",0.0);

}
function Control() {
	//toc do
	currentSpeed=2*22/7*wheelRL.radius*wheelRL.rpm*60/1000;
	 currentSpeed=Mathf.Round(currentSpeed);
	 if(currentSpeed<topSpeed&&currentSpeed>-maxReverseSpeed&&!braked){
	 	 wheelRR.motorTorque=maxTorque*Input.GetAxis("Vertical");
		 wheelRL.motorTorque=maxTorque*Input.GetAxis("Vertical");
	 }else{
	 	 wheelRR.motorTorque=0;
		 wheelRL.motorTorque=0;
	 }
	 if(Input.GetButton("Vertical")==false){
	 	 wheelRR.brakeTorque=decelerationSpeed;
		wheelRL.brakeTorque=decelerationSpeed;
	 }else{
	 wheelRR.brakeTorque=0;
		wheelRL.brakeTorque=0;
	 }
	 var speedFactor=GetComponent.<Rigidbody>().velocity.magnitude/lowestSteerAtSpeed;
	 var currentSterAngel=Mathf.Lerp(lowSpeedSteerAngel,highSpeedSteerAngel,speedFactor);

	 wheelFR.steerAngle=currentSterAngel;
	 wheelFL.steerAngle=currentSterAngel;
}
function HandBrake(){
	if(Input.GetButton("Jump")){
		braked=true;
	}else{
		braked=false;
	}
	if(braked){
		wheelFR.brakeTorque=maxBrakeTorque;
		wheelFL.brakeTorque=maxBrakeTorque;
		wheelRR.motorTorque=0;
		wheelRL.motorTorque=0;
		if(GetComponent.<Rigidbody>().velocity.magnitude>1){
		wheelRR.forwardFriction.stiffness=slipForwardFriction;
		wheelRL.forwardFriction.stiffness=slipForwardFriction;

		wheelRR.sidewaysFriction.stiffness=slipSidewayFriction;
		wheelRL.sidewaysFriction.stiffness=slipSidewayFriction;
			//SetSlip(slipForwardFriction,slipSidewayFriction);
		}else{
			//SetSlip(1,1);
			wheelRR.forwardFriction.stiffness=1;
	       wheelRL.forwardFriction.stiffness=1;

		wheelRR.sidewaysFriction.stiffness=1;
		wheelRL.sidewaysFriction.stiffness=1;

		}
	}else{
		wheelFR.brakeTorque=0;
		wheelFL.brakeTorque=0;
		//SetSlip(myForwardFriction,mySidewayFriction);

		wheelRR.forwardFriction.stiffness=myForwardFriction;
		wheelRL.forwardFriction.stiffness=myForwardFriction;

		wheelRR.sidewaysFriction.stiffness=mySidewayFriction;
		wheelRL.sidewaysFriction.stiffness=mySidewayFriction;

	}

	
	
	//function SetSlip ( currentForwardFriction : float , currentSidewayFriction : float) {
		//wheelRR.forwardFriction.stiffness=currentForwardFriction;
		//wheelRl.forwardFriction.stiffness=currentForwardFriction;

		//wheelRR.sidewaysFriction.stiffness=currentSidewayFriction;
		//wheelRL.sidewaysFriction.stiffness=currentSidewayFriction;
	//}

	}