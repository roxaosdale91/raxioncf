namespace GameCreator.Melee
{
	using UnityEngine;

	public class InputBuffer
	{
        public float timeWindow;

        protected float inputTime;
        protected float comboTime;
        protected float downTime;
        protected CharacterMelee.ActionKey key;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public InputBuffer(float timeWindow)
        {
            this.timeWindow = timeWindow;

            this.inputTime = -100f;
            this.comboTime = -100f;
            this.downTime = -100f;
            this.key = CharacterMelee.ActionKey.A;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual void AddInput(CharacterMelee.ActionKey key)
        {
            this.key = key;
            this.inputTime = Time.time;
        }

        public virtual void ComboTriggered()
        {
            this.comboTime = Time.time;
        }

        
        public virtual void KnockDown()
        {
            this.downTime = Time.time;
        }

        public virtual bool HasInput()
        {
            if (this.inputTime <= 0f) return false;
            return Time.time - this.inputTime <= this.timeWindow;
        }
        
        public virtual bool DidCombo()
        {
            // if (this.comboTime <= 0f) return false;
            var getComboTime = Time.time - this.comboTime;
            return getComboTime <= this.timeWindow;
        }
        
        public virtual bool WasKnockedDown()
        {
            // if (this.comboTime <= 0f) return false;
            var getDownTime = Time.time - this.downTime;
            return getDownTime <= this.timeWindow;
        }

        public CharacterMelee.ActionKey GetInput()
        {
            return this.key;
        }

        public void ConsumeInput()
        {
            this.inputTime = -100f;
        }

        public void ConsumeCombo()
        {
            this.inputTime = -100f;
        }
        
        public void ConsumeDown()
        {
            this.inputTime = -100f;
        }
	}

    // public class InputBufferv2
	// {
    //     public float timeWindow;

    //     protected float inputTime;
    //     protected CharacterMelee.ActionKey key;

    //     // CONSTRUCTOR: ---------------------------------------------------------------------------

    //     public InputBuffer(float timeWindow)
    //     {
    //         this.timeWindow = timeWindow;

    //         this.inputTime = -100f;
    //         this.key = CharacterMelee.ActionKey.A;
    //     }

    //     // PUBLIC METHODS: ------------------------------------------------------------------------

    //     public virtual void AddInput(CharacterMelee.ActionKey key)
    //     {
    //         this.key = key;
    //         this.inputTime = Time.time;
    //     }

    //     public virtual bool HasInput()
    //     {
    //         if (this.inputTime <= 0f) return false;
    //         return Time.time - this.inputTime <= this.timeWindow;
    //     }

    //     public CharacterMelee.ActionKey GetInput()
    //     {
    //         return this.key;
    //     }

    //     public void ConsumeInput()
    //     {
    //         this.inputTime = -100f;
    //     }
	// }
}