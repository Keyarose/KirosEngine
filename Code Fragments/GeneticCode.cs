namespace KirosProject
{
    public class GeneticCode
    {
        //Active genetics
        //General
        private float _heightA;
        private BodyType _bodyA;
        private SkinType _skinTypeA;
        private Color3 _skinColorA;
        private Color3 _hairColorA;
        private DietType _dietTypeA;
        private RespirationType _respTypeA;
        private float _globalGrowthRateA;
        
        //Head
        private HeadShape _headShapeA;
        private FaceShape _faceShapeA;
        
        private int _eyeCountA;
        private EyeShape _eyeShapeA;
        private EyeType _eyeTypeA;
        private IrisShape _irisShapeA;
        private Color3[] _irisColorsA; //array length will be _eyeCountA
        private Color3[] _eyeColorsA;
        private VisionRange _visionRangeA;
        
        private int _earCountA;
        private EarType _earTypeA;
        
        private NoseType _noseTypeA;
        
        private MouthType _mouthTypeA;
        private ToothType _toothTypeA;
        
        private HornType _hornTypeA;
        private int _hornCountA;
        
        //Appendages
        private int _armCountA;
        private int _fingerCountA;
        private int _legCountA;
        private FootType _footTypeA;
        private int _toeCountA;
        
        private NailType _nailTypeFingerA;
        private NailType _nailTypeToeA;
        
        //Reproduction
        private ReproductionMethod _reproductionMethodA;
        
        //Recesive genetics
        //General
        private float _heightR;
        private BodyType _bodyR;
        private SkinType _skinTypeR;
        private Color3 _skinColorR;
        private Color3 _hairColorR;
        private DietType _dietTypeR;
        private RespirationType _respTypeR;
        private float _globalGrowthRateR;
        
        //Head
        private HeadShape _headShapeR;
        private FaceShape _faceShapeR;
        
        private int _eyeCountR;
        private EyeShape _eyeShapeR;
        private EyeType _eyeTypeR;
        private IrisShape _irisShapeR;
        private Color3[] _irisColorsR;
        private Color3[] _eyeColorsR;
        private VisionRange _visionRangeR;
        
        private int _earCountR;
        private EarType _earTypeR;
        
        private NoseType _noseTypeR;
        
        private MouthType _mouthTypeR;
        private ToothType _toothTypeR;
        
        private HornType _hornTypeR;
        private int _hornCountR;
        
        //Appendages
        private int _armCountR;
        private int _fingerCountR;
        private int _legCountR;
        private FootType _footTypeR;
        private int _toeCountR;
        
        private NailType _nailTypeFingerR;
        private NailType _nailTypeToeR;
        
        //Reproduction
        private ReproductionMethod _reproductionMethodR;
        
        /// <summary>
        /// Expected maximum height for active genetics
        /// </summary>
        public float ActiveHeight
        {
            get
            {
                return _heightA;
            }
            set
            {
                _height = value;
            }
        }
        
        /// <summary>
        /// Body type for active genetics
        /// </summary>
        public BodyType ActiveBodyType
        {
            get
            {
                return _bodyA;
            }
            set
            {
                _bodyA = value;
            }
        }
        
        public SkinType ActiveSkinType
        {
            get
            {
                return _skinTypeA;
            }
            set
            {
                _skinTypeA = value;
            }
        }
    }
    
    /// <summary>
    /// The type of skin
    /// </summary>
    public enum SkinType
    {
        Smooth, //human like
        Scaled, //reptilian
        Furred
    }
    
    /// <summary>
    /// The type of nails on the digits
    /// </summary>
    [Flags]
    public enum NailType
    {
        None = 0,
        Short = 1 << 0, //Human length
        Medium = 1 << 1,
        Long = 1 << 2,
        Soft = 1 << 3, //useable with length not combineable with hard
        Hard = 1 << 4 //useable with length not combineable with soft
    }
    
    /// <summary>
    /// The type of ear
    /// </summary>
    [Flags]
    public enum EarType
    {
        Hole = 1 << 0, //no exterior structure
        Rounded = 1 << 1, //human like
        Pointed = 1 << 2, //elf like
        Furred = 1 << 3 //if there is fur or not on the ears, combined with the others
    }
    
    public enum ToothType
    {
        //TODO: types of teeth
    }
    
    /// <summary>
    /// The type of mouth
    /// </summary>
    public enum MouthType
    {
        Cheeked, //human like
        Cheekless, //dog like
        Lipless, //no lips no cheeks, teeth showing
        Pincered //insect like
    }
    
    /// <summary>
    /// The parts of the em spectrum that are visable to the eye
    /// acronims from wikipedia: Electromagnetic spectrum
    /// </summary>
    [Flags]
    public enum VisionRange
    {
        None = 0,
        FIR = 1 << 0,
        MIR = 1 << 1,
        NIR = 1 << 2,
        Visable = 1 << 3,
        EUV = 1 << 4,
        SX = 1 << 5,
        HX = 1 << 6
    }
    
    /// <summary>
    /// The type of eye
    /// </summary>
    public enum EyeType
    {
        VertebrateEye,
        CompoundEye
    }
    
    /// <summary>
    /// The Cephalic index of the head
    /// </summary>
    public enum HeadShape
    {
        Long,
        Medium,
        Short
    }
    
    /// <summary>
    /// The type of foot structure
    /// </summary>
    public enum FootType
    {
        Plantigrade,
        Digitigrade,
        Unguligrade
    }
}