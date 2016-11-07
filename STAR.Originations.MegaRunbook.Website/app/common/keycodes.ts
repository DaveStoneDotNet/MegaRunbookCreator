
export class KeyCodes {

    static get KEY_BACK_SPACE():    number { return   8; }
    static get KEY_TAB():           number { return   9; }
    static get KEY_ENTER():         number { return  13; }
    static get KEY_SHIFT():         number { return  16; }
    static get KEY_CONTROL():       number { return  17; }      // ctrlKey: true
    static get KEY_ALT():           number { return  18; }      // altKey:  true
    static get KEY_PAUSE():         number { return  19; }
    static get KEY_CAPS_LOCK():     number { return  20; }
    static get KEY_ESCAPE():        number { return  27; }
    static get KEY_SPACE():         number { return  32; }
    static get KEY_PAGE_UP():       number { return  33; }
    static get KEY_PAGE_DOWN():     number { return  34; }
    static get KEY_END():           number { return  35; }
    static get KEY_HOME():          number { return  36; }
    static get KEY_LEFT():          number { return  37; }
    static get KEY_UP():            number { return  38; }
    static get KEY_RIGHT():         number { return  39; }
    static get KEY_DOWN():          number { return  40; }
    static get KEY_INSERT():        number { return  45; }
    static get KEY_DELETE():        number { return  46; }

    static get KEY_0():             number { return  48; }
    static get KEY_1():             number { return  49; }
    static get KEY_2():             number { return  50; }
    static get KEY_3():             number { return  51; }
    static get KEY_4():             number { return  52; }
    static get KEY_5():             number { return  53; }
    static get KEY_6():             number { return  54; }
    static get KEY_7():             number { return  55; }
    static get KEY_8():             number { return  56; }
    static get KEY_9():             number { return  57; }

    static get KEY_A():             number { return  65; }
    static get KEY_B():             number { return  66; }
    static get KEY_C():             number { return  67; }
    static get KEY_D():             number { return  68; }
    static get KEY_E():             number { return  69; }
    static get KEY_F():             number { return  70; }
    static get KEY_G():             number { return  71; }
    static get KEY_H():             number { return  72; }
    static get KEY_I():             number { return  73; }
    static get KEY_J():             number { return  74; }
    static get KEY_K():             number { return  75; }
    static get KEY_L():             number { return  76; }
    static get KEY_M():             number { return  77; }
    static get KEY_N():             number { return  78; }
    static get KEY_O():             number { return  79; }
    static get KEY_P():             number { return  80; }
    static get KEY_Q():             number { return  81; }
    static get KEY_R():             number { return  82; }
    static get KEY_S():             number { return  83; }
    static get KEY_T():             number { return  84; }
    static get KEY_U():             number { return  85; }
    static get KEY_V():             number { return  86; }
    static get KEY_W():             number { return  87; }
    static get KEY_X():             number { return  88; }
    static get KEY_Y():             number { return  89; }
    static get KEY_Z():             number { return  90; }

    static get KEY_META():          number { return  91; }
    static get KEY_CONTEXT_MENU():  number { return  93; }

    static get KEY_NUMPAD_0():      number { return  96; }
    static get KEY_NUMPAD_1():      number { return  97; }
    static get KEY_NUMPAD_2():      number { return  98; }
    static get KEY_NUMPAD_3():      number { return  99; }
    static get KEY_NUMPAD_4():      number { return 100; }
    static get KEY_NUMPAD_5():      number { return 101; }
    static get KEY_NUMPAD_6():      number { return 102; }
    static get KEY_NUMPAD_7():      number { return 103; }
    static get KEY_NUMPAD_8():      number { return 104; }
    static get KEY_NUMPAD_9():      number { return 105; }

    static get KEY_MULTIPLY():      number { return 106; }       // *
    static get KEY_ADD():           number { return 107; }       // +
    static get KEY_SEPARATOR():     number { return 108; }       // 
    static get KEY_SUBTRACT():      number { return 109; }       // +
    static get KEY_DECIMAL():       number { return 110; }       // SEE PERIOD 190
    static get KEY_DIVIDE():        number { return 111; }       // SEE SLASH  191
    
    static get KEY_F1():            number { return 112; }
    static get KEY_F2():            number { return 113; }
    static get KEY_F3():            number { return 114; }
    static get KEY_F4():            number { return 115; }
    static get KEY_F5():            number { return 116; }
    static get KEY_F6():            number { return 117; }
    static get KEY_F7():            number { return 118; }
    static get KEY_F8():            number { return 119; }
    static get KEY_F9():            number { return 120; }
    static get KEY_F10():           number { return 121; }
    static get KEY_F11():           number { return 122; }
    static get KEY_F12():           number { return 123; }
    static get KEY_F13():           number { return 124; }
    static get KEY_F14():           number { return 125; }
    static get KEY_F15():           number { return 126; }
    static get KEY_F16():           number { return 127; }
    static get KEY_F17():           number { return 128; }
    static get KEY_F18():           number { return 129; }
    static get KEY_F19():           number { return 130; }
    static get KEY_F20():           number { return 131; }
    static get KEY_F21():           number { return 132; }
    static get KEY_F22():           number { return 133; }
    static get KEY_F23():           number { return 134; }
    static get KEY_F24():           number { return 135; }
    
    static get KEY_NUM_LOCK():      number { return 144; }
    static get KEY_SCROLL_LOCK():   number { return 145; }

    static get KEY_SEMICOLON():     number { return 186; }       // ;
    static get KEY_EQUALS():        number { return 187; }       // =
    static get KEY_COMMA():         number { return 188; }       // ,
    static get KEY_PERIOD():        number { return 190; }       // . SEE DECIMAL 110
    static get KEY_SLASH():         number { return 191; }       // / SEE DIVIDE  111
    static get KEY_BACK_QUOTE():    number { return 192; }       // `
    static get KEY_OPEN_BRACKET():  number { return 219; }       // [
    static get KEY_BACK_SLASH():    number { return 220; }       // \
    static get KEY_CLOSE_BRACKET(): number { return 221; }       // ]
    static get KEY_QUOTE():         number { return 222; }       // '

    digits: Array<number> = new Array();
    chars: Array<number> = new Array();
    alphaNumeric: Array<number> = new Array();
    editKeys: Array<number> = new Array();
    navKeys: Array<number> = new Array();
    modifierKeys: Array<number> = new Array();
    functionKeys: Array<number> = new Array();

    constructor() {
        this.initialize();
    }

    initialize() {
        this.initializeDigits();
        this.initializeChars();
        this.initializeAlphaNumeric();
        this.initializeEditKeys();
        this.initializeNavKeys();
        this.initializeModifierKeys();
        this.initializeFunctionKeys();
    }

    public initializeDigits() {

        this.digits.push(KeyCodes.KEY_0);
        this.digits.push(KeyCodes.KEY_1);
        this.digits.push(KeyCodes.KEY_2);
        this.digits.push(KeyCodes.KEY_3);
        this.digits.push(KeyCodes.KEY_4);
        this.digits.push(KeyCodes.KEY_5);
        this.digits.push(KeyCodes.KEY_6);
        this.digits.push(KeyCodes.KEY_7);
        this.digits.push(KeyCodes.KEY_8);
        this.digits.push(KeyCodes.KEY_9);

        this.digits.push(KeyCodes.KEY_NUMPAD_0);
        this.digits.push(KeyCodes.KEY_NUMPAD_1);
        this.digits.push(KeyCodes.KEY_NUMPAD_2);
        this.digits.push(KeyCodes.KEY_NUMPAD_3);
        this.digits.push(KeyCodes.KEY_NUMPAD_4);
        this.digits.push(KeyCodes.KEY_NUMPAD_5);
        this.digits.push(KeyCodes.KEY_NUMPAD_6);
        this.digits.push(KeyCodes.KEY_NUMPAD_7);
        this.digits.push(KeyCodes.KEY_NUMPAD_8);
        this.digits.push(KeyCodes.KEY_NUMPAD_9);
    } 

    public initializeChars() {

        this.chars.push(KeyCodes.KEY_A);
        this.chars.push(KeyCodes.KEY_B);
        this.chars.push(KeyCodes.KEY_C);
        this.chars.push(KeyCodes.KEY_D);
        this.chars.push(KeyCodes.KEY_E);
        this.chars.push(KeyCodes.KEY_F);
        this.chars.push(KeyCodes.KEY_G);
        this.chars.push(KeyCodes.KEY_H);
        this.chars.push(KeyCodes.KEY_I);
        this.chars.push(KeyCodes.KEY_J);
        this.chars.push(KeyCodes.KEY_K);
        this.chars.push(KeyCodes.KEY_L);
        this.chars.push(KeyCodes.KEY_M);
        this.chars.push(KeyCodes.KEY_N);
        this.chars.push(KeyCodes.KEY_O);
        this.chars.push(KeyCodes.KEY_P);
        this.chars.push(KeyCodes.KEY_Q);
        this.chars.push(KeyCodes.KEY_R);
        this.chars.push(KeyCodes.KEY_S);
        this.chars.push(KeyCodes.KEY_T);
        this.chars.push(KeyCodes.KEY_U);
        this.chars.push(KeyCodes.KEY_V);
        this.chars.push(KeyCodes.KEY_W);
        this.chars.push(KeyCodes.KEY_X);
        this.chars.push(KeyCodes.KEY_Y);
        this.chars.push(KeyCodes.KEY_Z);
    } 

    public initializeAlphaNumeric() {
        this.alphaNumeric.push.apply(this.alphaNumeric, this.digits);
        this.alphaNumeric.push.apply(this.alphaNumeric, this.chars);
    } 

    public initializeEditKeys() {

        this.editKeys.push(KeyCodes.KEY_BACK_SPACE);
        this.editKeys.push(KeyCodes.KEY_DELETE);
    } 

    public initializeNavKeys() {

        this.navKeys.push(KeyCodes.KEY_TAB);
        this.navKeys.push(KeyCodes.KEY_LEFT);
        this.navKeys.push(KeyCodes.KEY_RIGHT);
        this.navKeys.push(KeyCodes.KEY_UP);
        this.navKeys.push(KeyCodes.KEY_DOWN);
        this.navKeys.push(KeyCodes.KEY_END);
        this.navKeys.push(KeyCodes.KEY_HOME);
        this.navKeys.push(KeyCodes.KEY_PAGE_UP);
        this.navKeys.push(KeyCodes.KEY_PAGE_DOWN);
    }

    public initializeModifierKeys() {

        this.modifierKeys.push(KeyCodes.KEY_CONTROL);
        this.modifierKeys.push(KeyCodes.KEY_ALT);
        this.modifierKeys.push(KeyCodes.KEY_SHIFT);
        this.modifierKeys.push(KeyCodes.KEY_CAPS_LOCK);
    }

    public initializeFunctionKeys() {

        this.functionKeys.push(KeyCodes.KEY_F1);
        this.functionKeys.push(KeyCodes.KEY_F2);
        this.functionKeys.push(KeyCodes.KEY_F3);
        this.functionKeys.push(KeyCodes.KEY_F4);
        this.functionKeys.push(KeyCodes.KEY_F5);
        this.functionKeys.push(KeyCodes.KEY_F6);
        this.functionKeys.push(KeyCodes.KEY_F7);
        this.functionKeys.push(KeyCodes.KEY_F8);
        this.functionKeys.push(KeyCodes.KEY_F9);
        this.functionKeys.push(KeyCodes.KEY_F10);
        this.functionKeys.push(KeyCodes.KEY_F11);
        this.functionKeys.push(KeyCodes.KEY_F12);
        this.functionKeys.push(KeyCodes.KEY_F13);
        this.functionKeys.push(KeyCodes.KEY_F14);
        this.functionKeys.push(KeyCodes.KEY_F15);
        this.functionKeys.push(KeyCodes.KEY_F16);
        this.functionKeys.push(KeyCodes.KEY_F17);
        this.functionKeys.push(KeyCodes.KEY_F18);
        this.functionKeys.push(KeyCodes.KEY_F19);
        this.functionKeys.push(KeyCodes.KEY_F20);
        this.functionKeys.push(KeyCodes.KEY_F21);
        this.functionKeys.push(KeyCodes.KEY_F22);
        this.functionKeys.push(KeyCodes.KEY_F23);
        this.functionKeys.push(KeyCodes.KEY_F24);
    }

    isDigit(key) {
        return this.digits.find(k => k === key)
    }

    isChar(key) {
        return this.chars.find(k => k === key)
    }

    isAlphaNumeric(key) {
        return this.alphaNumeric.find(k => k === key)
    }

    isEditKey(key) {
        return this.editKeys.find(k => k === key)
    }

    isNavKey(key) {
        return this.navKeys.find(k => k === key)
    }

    isModifierKey(key) {
        return this.modifierKeys.find(k => k === key)
    }

    isFunctionKey(key) {
        return this.functionKeys.find(k => k === key)
    }
}