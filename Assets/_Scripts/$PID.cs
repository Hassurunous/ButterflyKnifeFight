public class customPID {
	public float pFactor, iFactor, dFactor;
		
	float integral;
	float lastError;
	
	
	public customPID(float p, float i, float d) {
		this.pFactor = p;
		this.iFactor = i;
		this.dFactor = d;
	}
	
	
	public float Update(float setpoint, float actual, float timeFrame) {
		float present = setpoint - actual;
		integral += present * timeFrame;
		float deriv = (present - lastError) / timeFrame;
		lastError = present;
		return present * pFactor + integral * iFactor + deriv * dFactor;
	}
}
