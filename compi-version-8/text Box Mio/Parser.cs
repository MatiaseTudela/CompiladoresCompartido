using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
//using at.jku.ssw.cc.tests;
using text_Box_Mio;
//using compilador;

namespace at.jku.ssw.cc {
  public class ElemPilita
    {
        public enum ElemDePila { esEntero, esEstring };
        public int entero;
        public string estring;
        public ElemDePila elemDePila;
        //public static BrtrueENUM vsrEnum = BrtrueENUM.BEQenum;
        public ElemPilita(int entero, string estring, ElemDePila elemDePila)
             { this.entero = entero; this.estring = estring; this.elemDePila = elemDePila; }
    }

  public class PilaMia : Pila
  {
     public  PilaMia(int cantElem):base(cantElem) { }

     public void mostrarPilita()
     {
         string pilaString = "";
         for (int i = this.cantMaxDeElem - 1; i >= 0; i--)
         {
             if (i > Parser.pilita.tope)

                 pilaString = pilaString + "\n" + " ";
             else
             {
                 string elemParaMostrar;
                 if (((ElemPilita)elementos[i]).elemDePila == ElemPilita.ElemDePila.esEntero)
                     elemParaMostrar = ((ElemPilita)elementos[i]).entero.ToString();
                 else elemParaMostrar = ((ElemPilita)elementos[i]).estring;
                 pilaString = pilaString + "\n" + elemParaMostrar;

             }
         }
         Program1.form1.richTextBox2.Text = pilaString;
         if (Parser.muestraCargaDeInstrs) Parser.MessageBoxCon2PregMaqVirtual();
     }

     public void inicializa()
     {
         for (int i = this.cantMaxDeElem - 1; i >= 0; i--)
         {
             elementos[i] = new ElemPilita(0, "", ElemPilita.ElemDePila.esEntero);
         }
     }

  }

  class Parser
   {
    public static FrmContinuar mens = new FrmContinuar();
    public static FrmContinuarMaqVirtual mensMaqVirtual = new FrmContinuarMaqVirtual();
    
    public static bool muestraProducciones = true;
    public static bool muestraCargaDeInstrs = true;
    public static bool ejecuta = false;

    public static PilaMia pilita = new PilaMia(10);


    public static void MessageBoxCon2PregMaqVirtual()
    {
        mensMaqVirtual.ShowDialog();
    }

    public static void MessageBoxCon3Preg()
    {
        if (muestraProducciones) //System.Windows.Forms.MessageBox.Show("Continuar","Árbol de Derivación");
            Program1.form1.instContinuar.ShowDialog();     
    

            Program1.form1.treeView1.ExpandAll();   
       // mens.ShowDialog();
    }//Fin MessageBoxCon3Preg()


    public enum AccionInstr { nop, loadLocal, storeLocal, add, sub, mul, div, pop, 
                                    loadConst, write, writeln, branchInc, tJump, fJump, ldstr, ret }

    public class Instruccion
    {
        public AccionInstr accionInstr;
        public int nro;
        public int nroLinea;
        public string argDelWriteLine;
        public Code.BrfalseENUM indBrFalse;
        public Code.BrtrueENUM indBrTrue;
        public string instrString;


        public Instruccion(AccionInstr accionInstr, int nro, int nroLinea, string argDelWriteLine, 
                           Code.BrtrueENUM indBrTrue, Code.BrfalseENUM indBrFalse, string instrString )
        { this.accionInstr = accionInstr; this.nro = nro;
          this.nroLinea = nroLinea; this.argDelWriteLine = argDelWriteLine;
          this.indBrTrue = indBrTrue; this.indBrFalse = indBrFalse; this.instrString = instrString;
        }
    }
    public static bool yaPintada = false;
    public const int maxnroDeInstrCorriente = 200;
    public static int nroDeInstrCorriente; //nroDeInstrCorriente;
//    public static int nroInstrParaRectificarElWhile; 
    public static Instruccion[] cil; 
    static string[] namesAccionInstr = { "nop", "ldloc.", "stloc.", "add", "sub", "mul", "div", "pop",
                                         "ldc.i4 ", "write", "writeln", "br", "br_tJump" ,
                                         "br_fJuamp", "ldstr", "ret"};

    public static void inicializaCil()
    {
        cil = new Instruccion[maxnroDeInstrCorriente];
        for (int i = 0; i < maxnroDeInstrCorriente; i++)
            cil[i] = new Instruccion(AccionInstr.nop, 0, 0,"",
                                     Code.BrtrueENUM.BEQenum, Code.BrfalseENUM.BEQenum, "");
    }

    public static void muestraCil()
    {
        string todasLasInstrs = "Accion\tNro\tLinea";
        System.Windows.Forms.MessageBox.Show(maxnroDeInstrCorriente.ToString());

        for (int i = 0; i < maxnroDeInstrCorriente; i++)
        {
            todasLasInstrs = todasLasInstrs + "\n"
                                + i.ToString() + ":"
                                + namesAccionInstr[(int)cil[i].accionInstr] + "\t"
                                + cil[i].nro.ToString() + "\t"
                                + cil[i].nroLinea.ToString()
                                + cil[i].argDelWriteLine;
        }
        Program1.form1.richTextBox5.Text = todasLasInstrs;
        System.Windows.Forms.MessageBox.Show("fin");
    }

    public const int maxCantVarsLocales = 10;
    public static int cantVarLocales; 
    public static int[] locals = new int[maxCantVarsLocales];

    public static void muestraVarsLocales()
    {
        string todasLasVarsLocales;
        if (cantVarLocales == 0) todasLasVarsLocales = "No hay vars locales";
        else
        {
            todasLasVarsLocales = locals[0].ToString();
            for (int i = 1; i < cantVarLocales; i++)
            {
                todasLasVarsLocales = todasLasVarsLocales + "\n" + locals[i].ToString();
            }
            Program1.form1.richTextBox4.Text = todasLasVarsLocales;
        }
    }

    static void ParteFinal1()
    {
        Type ptType1 = Code.program.CreateType(); 
        object ptInstance1 =
                Activator.CreateInstance(ptType1, new object[0]);  //new object[0] si sin parms
        ptType1.InvokeMember("Main", BindingFlags.InvokeMethod, null, ptInstance1, new object[0]);
        Code.assembly.Save("Piripipi" + ".exe");
        //Console.WriteLine("\nTermina ret, create type, createInst, Invoke Main");
        
        if (ZZ.readKey) Console.ReadKey();
    }//Fin ParteFinal1

    static void ReadKeyMio () 
        {
                Code.il.EmitCall(OpCodes.Call, typeof(Console).GetMethod("Read", new Type[0]), null);
                Code.il.Emit(OpCodes.Conv_U2);
                Code.il.Emit(Code.POP);
        }

    public const int
        PROGRAM = 1, CONSTDECL = 2, VARDECL = 3, CLASSDECL = 4, METHODDECL = 5, FORMPARS = 6,
        TYPE = 7, STATEMENT = 8, BLOCK = 9, ACTPARS = 10, CONDITION = 11, CONDTERM = 12,
        CONDFACT = 13, EXPR = 14, TERM = 15, FACTOR = 16, DESIGNATOR = 17, RELOP = 18, 
        ADDOP = 19, MULOP = 20, IDENT=21;

	static TextWriter output;

	public static Token token;    // last recognized token
    public static Token laToken;  // lookahead token (not yet recognized)
	static int la;         // shortcut to kind attribute of lookahead token (laToken.kind)

	/* Symbol table object of currently compiled method. */
	internal static Symbol curMethod;
	
	/* Special Label to represent an undefined break destination. */
	//static readonly Label undef;
	
	/* Reads ahead one symbol. */
	static void Scan () {   
        token = laToken;
        laToken = Scanner.Next(); 
        //La 1° vez q se ejecuta, token queda con Token(1, 1), laToken con "class" (primer token del programa)
                                             
        //Console.WriteLine("token.kind: "+Token.names[token.kind]+ "\t token.str: " + token.str);
        //Console.WriteLine("laToken.kind:"+Token.names[laToken.kind]+"\t laToken.str:"+laToken.str+"\n");
        la = laToken.kind;
	}
	
	/* Verifies symbol and reads ahead. */
	static void Check (int expected) {  //expected viene de la gramatica,  la del laToken que leyó
		if (la == expected) 
            Scan();
        else
            Errors.Error("Se esperaba un: " + Token.names[expected]); 
	}

 	//////////////////////////////////////////////////////////////////////////
	/* Program = "class" ident { ConstDecl | VarDecl | ClassDecl } 
	 *           "{" { MethodDecl } "}" . */
    // First(Program)={class};  Follow(Program)={eof}

	static void Program() {
        //Program1.form1.treeView1.ExpandAll();
        System.Windows.Forms.TreeNode program = new System.Windows.Forms.TreeNode();  //Crea el nodo program

        if (Parser.muestraProducciones)
        {

            program.Text = "Program"; //Texto del nodo "program"
            Program1.form1.treeView1.Nodes.Add(program); //"cuelga" el nodo (raiz) "program" del treeView1 ya creado

            Parser.MessageBoxCon3Preg();

            Code.seleccLaProdEnLaGram(0);  //pinta de rojo    Program = 'class' ident PosDeclars '{' MethodDeclsOpc '}'.

            //Code.cargaProgDeLaGram("Program = 'class' ident PosDeclars '{' MethodDeclsOpc '}'.");

            Parser.MessageBoxCon3Preg();

            program.Nodes.Add("class");
            //Program1.form1.treeView1.Refresh();
            //Program1.form1.treeView1.Update();
            program.ExpandAll(); //Visualiza (Expande) hijo de Program

            Parser.MessageBoxCon3Preg();
        }

        //antes del Check (Token.CLASS), token = ...(1,1),  laToken = ..."class".. y la = Token.CLASS
        Check(Token.CLASS);   //class ProgrPpal
        //Se cumple que:  (la == expected) => ejecuta Scan => token = ..."class"... y laToden = ..."ProgrPpal" 


        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);   //colorea "class" en ventana de Edicion
        //El argumento "true" => que lo que va a colorear es "el token" (en este caso: "class").
        //Si el arg es "false" => que lo que va a colorear es "el laToken" 

        
        //"Program = 'class' ident PosDeclars '{' MethodDeclsOpc '}'."
        //Ya reconoció 'class', ahora va a reconocer ident
        program.Nodes.Add("ident");
        Parser.MessageBoxCon3Preg();

        Check(Token.IDENT); // "ProgrPpal" => debo insertar el token en la tabla de símbolos
                            // es el comienzo del programa y abrir un nuevo alcance
        //Ahora token = "ProgrPpal" y laToken = "{"

        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);  //"class" ya lo pintó, ahora pinta "ProgrPpal"  (lo que hay en token)
                                              
        Symbol prog = Tab.Insert(Symbol.Kinds.Prog, token.str, Tab.noType);//lo cuelga de universe

        Code.CreateMetadata(prog);

        Tab.OpenScope(prog); //topScore queda ahora apuntando a un nuevo Scope
                         //El Scope anterior (universo) lo accedo via topScore.outer
        //Ya analizó Class ProgrPpal

        //Declaraciones (de ctes, de Globals(aunque diga de vars) y de clases) 
        //hasta q venga una "{"

        //"Program = 'class' ident PosDeclars '{' MethodDeclsOpc '}'."
        //Ya reconoció ident, ahora va a reconocer PosDeclars
        System.Windows.Forms.TreeNode posDeclars = new System.Windows.Forms.TreeNode("PosDeclars");
        program.Nodes.Add(posDeclars);  //Cuelga un TreeNode porque PosDeclars es No Terminal
        Parser.MessageBoxCon3Preg();

        Code.seleccLaProdEnLaGram(1);  //"PosDeclars = . | Declaration PosDeclars.";
        Parser.MessageBoxCon3Preg();
        
        //bool existeDecl = false;
        
        //bool bandarita = false; // bandera se encarga de verificar si existen declaraciones globales,constantes etc.
        //"Declaration = ConstDecl | VarDecl | ClassDecl."

        while (la != Token.LBRACE && la != Token.EOF) //Si no existen declaraciones, la = Token.LBRACE
        {
            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false); //si existiera una declaracion, as "int i", colorea "int";  (yaPintado = true)
            //El argumento "false" => que no debe pintar el "token" (que en este caso seria "ProgrPpal"), sino el laToken (que es "int")

            //en este caso, debe "mirar hacia adelante" (laToken) 
            //para determinar la opcion de la produccion "PosDeclars = . | Declaration PosDeclars."    
            //Si laToken es "{" => la opcion es "PosDeclars = .", otherwise: "PosDeclars = Declaration PosDeclars."

            //Code.cargaProgDeLaGram("PosDeclars = Declaration PosDeclars.");
            Code.seleccLaProdEnLaGram(2);

            //Code.cargaProgDeLaGram("Declaration = ConstDecl | VarDecl | ClassDecl.");
            System.Windows.Forms.TreeNode hijodeclar = new System.Windows.Forms.TreeNode("Declaration = ConstDecl | VarDecl | ClassDecl.");
            posDeclars.Nodes.Add(hijodeclar); 
            
            //existeDecl = true; 
            
            switch (la)
            {
                case Token.CONST:
                    ConstDecl(hijodeclar);
                    break;
                case Token.IDENT:  //Type ident..
                    //tipoSimbolo = Symbol.Kinds.Global;//debe ser para ident...
                   
                    Code.cargaProgDeLaGram("Declaration = VarDecl.");
                    System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("Declaration = VarDecl.");
                    hijodeclar.Nodes.Add(hijo1);
                    Code.seleccLaProdEnLaGram(6);
                    Code.cargaProgDeLaGram("VarDecl = Type  ident IdentifiersOpc ';'.");
                    Code.seleccLaProdEnLaGram(12);
                    
                    Code.cargaProgDeLaGram("Type = ident LbrackOpc."); //ya pintó el ident (por ej "int en int ii);
                    VardDecl(Symbol.Kinds.Global,hijo1); //En program  //Table val;

                    break;
                case Token.CLASS:
                    ClassDecl();/*No se encuentra la gramatica para implementar declaracione de clases" */
                    break;
                default: 
                    token = laToken;
                    Errors.Error("Se esperaba Const, Tipo, Class");
                    break;
            }
            Code.seleccLaProdEnLaGram(1);
            Code.cargaProgDeLaGram("selccionó la 1 PosDeclars = . | Declaration PosDeclars.");
        }

        Code.coloreaConRojo("latoken");
        //Code.coloreaConRojo(false); //"{"
        //El argumento "false" => que no debe pintar el "token" (que en este caso seria "ProgrPpal"), sino el laToken (que es "{")

        //en este caso, debe "mirar hacia adelante" (laToken) 
        //para determinar la opcion de la produccion "PosDeclars = . | Declaration PosDeclars."    
        //Si laToken es "{" => la opcion es "PosDeclars = .", otherwise: "PosDeclars = Declaration PosDeclars."

        if (la!=Token.EOF)
        {
            //Code.cargaProgDeLaGram("PosDeclars = .");////////////////////////
            posDeclars.Nodes.Add(".");
            posDeclars.ExpandAll(); //Visualiza (Expande) posDeclars
            Parser.MessageBoxCon3Preg(); 
        }
        if (ZZ.parser)
        {
            Console.WriteLine("Terminó con todas las declaraciones");
            Console.WriteLine("//el topScope queda apuntando a --> const size, class Table (con int[] pos y int[] neg), Table val");
        };
        if (ZZ.parser) { Console.WriteLine("empieza {"); if (ZZ.readKey) Console.ReadKey(); };

        Check(Token.LBRACE);
        //No es necesario pintar ya que el Token venia pintado desde antes
        //Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);  //ya lo pintó
        /////////
        program.Nodes.Add("'{'");
        ////////
            Parser.MessageBoxCon3Preg();

            System.Windows.Forms.TreeNode methodDeclsOpc = new System.Windows.Forms.TreeNode("MethodDeclsOpc");
            program.Nodes.Add(methodDeclsOpc);
            //////////////////
            Parser.MessageBoxCon3Preg();
            Code.seleccLaProdEnLaGram(3);//3.MethodDeclsOpc = . | MethodDecl Meth
            Parser.MessageBoxCon3Preg(); 

        // si "la" pertenece a First(MethodDec) => sólo deben haber metodos
        while ((la == Token.IDENT || la == Token.VOID) && la != Token.EOF)
        //////////////////////////////////////////////////////////////////////////////////////////////////
        {
            ///////////////////


            MethodDecl(methodDeclsOpc);  //void Main() int x,i; {val = new Table;....}
                     //hijo1 = "MethodDeclsOpc"
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////

        Check(Token.RBRACE);
        //Code.cargaProgDeLaGram("termina }"); //Ya estaba la produccion 
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);
        //////
        program.Nodes.Add("}");
        /////

        //Inicio Modificacion - Grupo 1 - 10/9/15
        //No es necesario mostrar este richtextbox y el boton hasta no ejecutar la maquina virtual
        //Program1.form1.richTextBox2.Visible = true;// new Program1.form1.richTextBox(); // 2.Visible; //   .Visible();
        //Program1.form1.button3.Visible = true;
        //Fin Modificacion - Grupo 1 - 10/9/15
        if (ZZ.parser)
        {
            Console.WriteLine("antes de prog.locals = Tab.topScope.locals; Tab.CloseScope()");
            if (ZZ.readKey) Console.ReadKey();
            //Tab.mostrarTab();
        };

        prog.locals = Tab.topScope.locals;

        if (ZZ.parser)
        {
            Console.WriteLine("mostrarSymbol");
            prog.mostrarSymbol("");
            if (ZZ.readKey) Console.ReadKey();
        };
        
        Tab.CloseScope();
        Tab.mostrarTab();

        bool Depuracion = false;
        if (!Depuracion) ParteFinal1();
        
        if (ZZ.parser)
        {
            Console.WriteLine("despues de prog.locals = Tab.topScope.locals; Tab.CloseScope()");
            if (ZZ.readKey) Console.ReadKey();
            Tab.mostrarTab();
        };
       
        


    }//Fin Program

	/* ConstDecl = "const" Type ident "=" ( number | charConst ) ";" . */
    //First(ConstDecl)={const}; Follow(ConstDecl)={"{"}
    static void ConstDecl(System.Windows.Forms.TreeNode padre)  
    {   //laToken=const  
        ////
        System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("Declaration = ConstDecl.");
        padre.Nodes.Add(hijo1);
        //////////
        System.Windows.Forms.TreeNode hijo2 = new System.Windows.Forms.TreeNode("ConstDecl = 'const' Type ident '=' NumberOrCharConst");
        hijo1.Nodes.Add(hijo2);
        Check(Token.CONST);  //const int i = 3;
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);
        /////
        hijo2.Nodes.Add("'const'");
        ////
        Code.cargaProgDeLaGram("Declarations = Declaration Declarations.");
        Code.cargaProgDeLaGram("Declaration = ConstDecl.");

        //token=const  laToken=int
        Struct type; 
        //= new Struct(Struct.Kinds.None); se podria haber hecho de 2 modos mas: ref y x valor
        // en ambos casos necesito Struct type = new...; 
        Type(out type); //En ConstDecl()  /
        /////
        hijo2.Nodes.Add("Type = "+type.kind.ToString());
        ////
        //Code.coloreaConRojo(true);
        if (type != Tab.intType && type != Tab.charType)  
        {
            Errors.Error("el tipo de una def de const sólo puede ser int o char");
        }          
        Check(Token.IDENT); //i
        ////
        hijo2.Nodes.Add("ident");
        ////
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true); 
        if (muestraProducciones) MessageBoxCon3Preg(); ;
        //token quedó con i, laToken con =
        // debo agregar a la tabla de símbolos un nuevo símbolo  
        Symbol constante = 
            Tab.Insert(Symbol.Kinds.Const, token.str, type);
        if (ZZ.parser) Console.WriteLine("termina de insertar constante con type = " + type.kind);

        Check(Token.ASSIGN);  //const
        ////
        hijo2.Nodes.Add("'='");
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true); 
        if (muestraProducciones) MessageBoxCon3Preg();
        //token quedó con =, laToken con 10
        ////
        System.Windows.Forms.TreeNode hijo3 = new System.Windows.Forms.TreeNode("NumberOrCharConst");
        hijo2.Nodes.Add(hijo3);
        switch (la)
        {
            case Token.NUMBER:
                if (type != Tab.intType) Errors.Error("type debe ser int");
                Check(Token.NUMBER);
                Code.coloreaConRojo("token");    
                //Code.coloreaConRojo(true);
                ////
                hijo3.Nodes.Add("number");
                ////
                if (muestraProducciones) MessageBoxCon3Preg();
                constante.val = token.val;
                break;
            case Token.CHARCONST:
                if (type != Tab.charType) Errors.Error("type debe ser char");
                Check(Token.CHARCONST);
                ////
                hijo3.Nodes.Add("charConst");
                ////
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true); if (muestraProducciones) MessageBoxCon3Preg();
                constante.val = token.val; //Seguro?
                break;
            default: Errors.Error("def de const erronea");
                break;
        };
        if (ZZ.parser) Console.WriteLine("Ahora actualizó el valor");
        if (ZZ.parser) Tab.mostrarTab();
        //Tab.mostrarTab();
        //Program1.form1.richTextBox10.Text = Tab.tabSimbString;

        Check(Token.SEMICOLON);
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);
        ////
        hijo2.Nodes.Add("';'");
        ////
    }
        

	//VarDecl = Type ident { "," ident } ";" . 
    // First(VarDecl)={ident}; Follow(VarDecl)={"{"}

    //  too puede ser un field, ademas de una ., o un Global
    static void VardDecl(Symbol.Kinds kind, System.Windows.Forms.TreeNode padre)
    {  //visto  //si es "int[] pos" y viene de "Class Tabla {", kind es "Field"
        // si es Tabla val y viene de "class P {", kind es "Global"
        Struct type;// = new Struct(Struct.Kinds.None); //int i;
        Code.seleccLaProdEnLaGram(6);
        if (muestraProducciones) MessageBoxCon3Preg();

        Code.cargaProgDeLaGram("VarDecl = Type  ident IdentifiersOpc ';'");
        /////

        //Cambiar desde aca aaaaaaaaaaa

        System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("VarDecl = Type  ident IdentifiersOpc ';'");
        padre.Nodes.Add(hijo1);
        /////
        Type(out type);  //En VardDecl
                //int[] en el caso del "int[] pos",... int, Table, Persona, int[], etc  
                //Table en el caso de Table val;
                //int en int x;
        ////
        hijo1.Nodes.Add("Type = "+type.kind.ToString());
        ////
        Check(Token.IDENT); // "pos", en int pos,   .....int,....  x, i, etc
        ////
        hijo1.Nodes.Add("ident");
        ////
        System.Windows.Forms.TreeNode hijo2 = new System.Windows.Forms.TreeNode("IdentifiersOpc");
        hijo1.Nodes.Add(hijo2);
        ////
        // debo insertar el token en la tabla de símbolos
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);  //Ya viene pintado
        cantVarLocales++; //provisorio: esto deberia hacerlo solo para el caso de var locales (no para var globales)
        Symbol vble = Tab.Insert(kind, token.str, type);   
        //vble no, poner simbolo (para pos, en int[] pos)
        //pues en este caso  es campo, y devuelve el Symbol p/pos,  type es int[]
        //puede ser val, en Tabla val, y type es Table //y devuelve el Symbol p/val
        Code.CreateMetadata(vble); //Para el campo pos (en int[] pos)Global, Field o .....  
                                   //o Para la vbe Global val
                                   //o para x en int x;

        Code.seleccLaProdEnLaGram(7);
        //Code.cargaProgDeLaGram("IdentifiersOpc = . | ',' ident IdentifiersOpc.");
        while (la == Token.COMMA && la != Token.EOF) 
        {
            Scan(); // Check(Token.COMMA);
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);
            Code.cargaProgDeLaGram("IdentifiersOpc = ',' ident  IdentifiersOpc.");//deberia extender el arbol
            ////
            hijo2.Nodes.Add("','");
            ////
            //if (muestraProducciones) MessageBoxCon3Preg();

            Check(Token.IDENT); //otro identif
            hijo2.Nodes.Add("ident");
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);
            cantVarLocales++; //provisorio: esto deberia hacerlo solo para el caso de var locales (no para var globales)

            //if (muestraProducciones) MessageBoxCon3Preg();

            vble = Tab.Insert(kind, token.str, type);
            Code.CreateMetadata(vble); //Para i, en int x,i
            Code.seleccLaProdEnLaGram(7);
            //Code.cargaProgDeLaGram("IdentifiersOpc = . | ',' ident IdentifiersOpc.....al semicolon");
        }
        Check(Token.SEMICOLON);
        hijo1.Nodes.Add("';'");
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);

        Code.cargaProgDeLaGram("IdentifiersOpc = .");
    }
    
    /* ClassDecl = "class" ident "{" { VarDecl } "}" . */
    // First(ClassDecl)={class}; Follow(ClassDecl)={"{"}
    static void ClassDecl()  
    {//class Table {int i; int j; metodo1...  metodo2...}
        Check(Token.CLASS); //class Table {int[] pos;int[] pos;},.. class C1 {int i; void P1(){}; char ch; int P2{}; int[] arr;  }
        Check(Token.IDENT); // "Table", laToken queda con "{"
        String nombreDeLaClase = token.str;//Table
        Struct StructParaLaClase = new Struct(Struct.Kinds.Class);
        //crear un Struct para la clase (con kind=Class)

        Check(Token.LBRACE); //"{"... laToken queda con "Table",...const    C1,  o  Table (en class Table)   
        Symbol nodoClase = Tab.Insert(Symbol.Kinds.Type, nombreDeLaClase, //crea symbol p/la clase Table
                                      StructParaLaClase);  //Con type=class

        //nodoClase.type =  StructParaLaClase; //no hace falta, ya lo hizo en el insert

        Code.CreateMetadata(nodoClase);//crea clase Din p/Table (por ej.), 
                                       //queda apuntada por nodoClase.type.sysType
        
        // todas las variables que declaren son tipo FIELD, excepto las clases (anidadas)
        //class C1 { => int i,j; char ch; Pers p=new Pers(); int P2{}; int[] arr;  }
        //class Table { => int[] pos; int[] neg}
        //por ahora, no permitimos void P1 {};

        Tab.OpenScope(nodoClase);
        while (la != Token.RBRACE && la != Token.EOF)  //itera p/c/campo (pos)
        {
            switch (la)
            {
                case Token.CONST:
                    ConstDecl(null);  //const int size = 10
                    break;
                case Token.IDENT:  //Type ident.., por ej: int i, int[] pos, etc...
                    //tipoSimbolo = Symbol.Kinds.Field...
                    //deberia ser declaracion de campo (pues cuelga de una clase, no de un metodo)
                    VardDecl(Symbol.Kinds.Field,null); // P/distinguir campos de clases
                         //Lo que va a declarar (pos), va a ser un Symbol con kind "field"
                                                  //int[] pos;
                    break;
                case Token.CLASS: //por ahora no debiera permitir clases anidadas ???
                    ClassDecl();
                    break;
                
                default:
                    Errors.Error("Se esperaba Const, tipo, class");
                    break;
            }
        }
        
        //crear un Struct para la clase (con kind=Class)
        //Struct StructParaLaClase = new Struct(Struct.Kinds.Class);

        //hacer StructParaLaClase.fields = topScope.locals
        StructParaLaClase.fields = Tab.topScope.locals;
        //nodoClase.type =  StructParaLaClase;

        Tab.CloseScope();  //con lo cual recuperamos topSScope 

        Check(Token.RBRACE);   //laToken queda con Table (en Table ...)

        //class C1 { => int i,j; char ch; Pers p=new Pers(); int P2{}; int[] arr;  }
        // int i,j; char ch; Pers p..etc, quedó apuntado por topScope.locals
    }


	/* MethodDecl = ( Type | "void" ) ident "(" [ FormPars ] ")"
	 *              { VarDecl } Block . 
	 */
    // First(MethodDecl)={ident, void}; Follow(MethodDecl)={"}"}
    static void MethodDecl(System.Windows.Forms.TreeNode padre) //void Main() int x,i; {val = new Table;....} 
    {
        /////// padre = "MethodDeclsOpc"
        System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("MethodDecl"); //luego hay que sacarlo y modificar la rama del ident
        //padre.Nodes.Add(hijo1);
        ////////
        //MessageBoxCon3Preg();
        System.Windows.Forms.TreeNode methodDecl = new System.Windows.Forms.TreeNode("MethodDecl"); //cuelga ESTE NODO DESPUES DE pintar el void


        Struct type = new Struct(Struct.Kinds.None); //Pone por defecto void
        if (la == Token.VOID || la == Token.IDENT)
        {
            if (la == Token.VOID)
            {
                
                Check(Token.VOID); //token = void laToken = Main
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);  //pinta void //incluye if (muestraProducciones) MessageBoxCon3Preg();
                //Code.cargaProgDeLaGram("MethodDeclsOpc = MethodDecl MethodDeclsOpc.");

                //Una vez que encuentra "void", infiere que debe usar la opcion: methodDecl.... etc 
                padre.Nodes.Add(methodDecl);
                padre.Expand();
                ////////
                if (muestraProducciones) MessageBoxCon3Preg();


                Code.seleccLaProdEnLaGram(8);  //MethodDecl = TypeOrVoid  ident "(" PossFormPars ")" Block.
                if (muestraProducciones) MessageBoxCon3Preg();

                System.Windows.Forms.TreeNode typeOrVoid = new System.Windows.Forms.TreeNode("TypeOrVoid"); //ya ha pintado el void
                methodDecl.Nodes.Add(typeOrVoid);
                methodDecl.Expand();
                if (muestraProducciones) MessageBoxCon3Preg();
                
                //Code.cargaProgDeLaGram("MethodDecl = TypeOrVoid ident '(' Pars ')' PosDeclars Block.");

                Code.seleccLaProdEnLaGram(9);  // 9.TypeOrVoid  = Type | "void“.
                if (muestraProducciones) MessageBoxCon3Preg();

                //como ya ha verificado "void", 
                
                typeOrVoid.Nodes.Add("'void'");
                typeOrVoid.Expand();
                if (muestraProducciones) MessageBoxCon3Preg();

                type = Tab.noType; //  para void
            }
            else
                if (la == Token.IDENT)
                {
                    Type(out type);  //  token = UnTipo laToken = Main
                    Code.cargaProgDeLaGram("TypeOrVoid  = Type.");
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    ///////////
                    //hay que cambiar hijo1 por methodDecl, e hijo2 por... 
                    System.Windows.Forms.TreeNode hijo2 = new System.Windows.Forms.TreeNode("TypeOrVoid = Type");
                    hijo1.Nodes.Add(hijo2);
                    hijo2.Nodes.Add("Type= "+type.kind.ToString());
                    //////////
                }
            ///////
            methodDecl.Nodes.Add("ident");
            if (muestraProducciones) MessageBoxCon3Preg();

            Check(Token.IDENT);  //Main por ej.  //token = Main, laToken = "("
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);
            
            //hijo1.Nodes.Add("ident");
            //////
            curMethod = Tab.Insert(Symbol.Kinds.Meth, token.str, type);//inserta void Main 
            // que pasa si hubieran parametros?
            Tab.OpenScope(curMethod);
            // tengo que insertar un método en la tabla de símbolo
            // todos los parámetros son locales 
            //tipoSimbolo = Symbol.Kinds.Local;
            ////
            methodDecl.Nodes.Add("'('");
            if (muestraProducciones) MessageBoxCon3Preg();
            ////

            
            Check(Token.LPAR);  //Si Main() => no tiene FormPars
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);


            /////////
            System.Windows.Forms.TreeNode pars = new System.Windows.Forms.TreeNode("Pars");
            methodDecl.Nodes.Add(pars);
            if (muestraProducciones) MessageBoxCon3Preg();

            Code.seleccLaProdEnLaGram(10);  //10.Pars = . | FormPar CommaFormParsOpc. 
            if (muestraProducciones) MessageBoxCon3Preg();

            if (la == Token.IDENT) //
            {
                
                FormPars(pars);  //Aqui hay que crear los symbolos para los args 

                methodDecl.Nodes.Add("')'");
                if (muestraProducciones) MessageBoxCon3Preg();

                Check(Token.RPAR);
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);  //pinta el ")"
            }
            //y colgarlos de curMethod.locals  
            else
            {
                //infiere que no hay params => 1) debe venir un ")". 2) La pocion de la produccion es "."
                //Code.cargaProgDeLaGram("Pars = .");
                Check(Token.RPAR);
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);  //pinta el ")"

                pars.Nodes.Add(".");
                pars.Expand();
                if (muestraProducciones) MessageBoxCon3Preg();

                methodDecl.Nodes.Add("')'");
                if (muestraProducciones) MessageBoxCon3Preg();
            }

            System.Windows.Forms.TreeNode posDeclars = new System.Windows.Forms.TreeNode("PosDeclars");
            methodDecl.Nodes.Add(posDeclars);
            if (muestraProducciones) MessageBoxCon3Preg();
            

            Code.seleccLaProdEnLaGram(1);  //1.Declarations =  .| Declaration Declarations.
            if (muestraProducciones) MessageBoxCon3Preg();
            
            //Code.cargaProgDeLaGram("PosDeclars =  .| Declaration PosDeclars.");
            ////////////
            System.Windows.Forms.TreeNode hijo22 = new System.Windows.Forms.TreeNode("PosDeclars");
            hijo1.Nodes.Add(hijo22);

            bool banderita=false;
            ///////////
            Code.CreateMetadata(curMethod);  //genera il
            //Declaraciones  por ahora solo decl de var, luego habria q agregar const y clases
            while (la != Token.LBRACE && la != Token.EOF) 
                //void Main()==> int x,i; {val = new Table;....}
            {
                //Code.cargaProgDeLaGram("Block =	'{'  StatementsOpc '}'.");
                
                
                if (la == Token.IDENT)
                {
                    banderita = true;
                    Code.coloreaConRojo("latoken");
                    //Code.coloreaConRojo(false); //colorea "int"  en int i; 

                    //Infiere la 2° opcion de PosDeclars   aaaaaaaa
                    System.Windows.Forms.TreeNode declaration = new System.Windows.Forms.TreeNode("Declaration");
                    posDeclars.Nodes.Add(declaration);
                    posDeclars.Expand();
                    if (muestraProducciones) MessageBoxCon3Preg();


                    //Code.cargaProgDeLaGram("PosDeclars = Declaration PosDeclars.");
                    Code.seleccLaProdEnLaGram(2);
                    if (muestraProducciones) MessageBoxCon3Preg();
                    //Code.cargaProgDeLaGram("Declaration = ConstDecl | VarDecl | ClassDecl.");

                    //Code.seleccLaProdEnLaGram(2);
                    Code.cargaProgDeLaGram("Declaration =  VarDecl.");
                    ////////
                    //Puesto q leyò un ident, infiere  q la opcion de la produccion es "VarDecl"

                    System.Windows.Forms.TreeNode varDecl = new System.Windows.Forms.TreeNode("VarDecl");
                    declaration.Nodes.Add(varDecl);
                    declaration.Expand();
                    if (muestraProducciones) MessageBoxCon3Preg();

                    ///////////
                    VardDecl(Symbol.Kinds.Local, varDecl); // int x,i; en MethodDecl()  con int ya consumido
                    //cantVarLocales++;
                    ///////
                    
                }
                else { token = laToken; Errors.Error("espero una declaracion de variable"); }
            }
            Code.seleccLaProdEnLaGram(1);
            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false); "{"
            if (banderita == false)
            {
                Code.cargaProgDeLaGram("PosDeclars =  .");
                hijo22.Nodes.Add("PosDeclars =  .");
            }
            //if (muestraProducciones) MessageBoxCon3Preg();

            if (cantVarLocales > 0)
            {
                string instrParaVarsLocs = ".locals init(int32 V_0"; 

                for (int i = 1; i < cantVarLocales; i++)
                {
                    instrParaVarsLocs = instrParaVarsLocs + "," + "\n          int32 V_" + i.ToString(); // +"  ";
                }
                instrParaVarsLocs = instrParaVarsLocs + ")";

                Code.cargaInstr(instrParaVarsLocs);
            }
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
              Block(hijo1);  //Bloque dentro de MethodDecl() 
//////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////
            curMethod.nArgs = Tab.topScope.nArgs;
            curMethod.nLocs = Tab.topScope.nLocs;
            curMethod.locals = Tab.topScope.locals;
            Tab.CloseScope();
            Tab.mostrarTab();

            Code.il.Emit(Code.RET);  //si lo saco se clava en el InvokeMember

            Parser.nroDeInstrCorriente++;
            Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.ret;
            Code.cargaInstr("ret");
        }
    }

	/* FormPars = Type ident { "," ident } . */
    // First(FormPars)={ident} ; Follow(FormPar)={")"}
    static void FormPars(System.Windows.Forms.TreeNode padre)///////////FALTA ARBOL
    {  //seguram hay que insertar el la Tab
        Struct type = new Struct(Struct.Kinds.None);

        if (la == Token.IDENT)
        {
            Type(out type); // 
            Code.seleccLaProdEnLaGram(5);
            //if (muestraProducciones) MessageBoxCon3Preg();

            Code.cargaProgDeLaGram("PossFormPars = FormPar CommaFormParsOpc.");
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);
            //if (muestraProducciones) MessageBoxCon3Preg();

            Check(Token.IDENT); //x
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);

            while (la == Token.COMMA && la != Token.EOF)
            {
                Check(Token.COMMA);
                
                Code.cargaProgDeLaGram("CommaFormParsOpc = ',' FormPar CommaFormParsOpc.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);

                Type(out type);
                Check(Token.IDENT);

                Code.seleccLaProdEnLaGram(6);

                Code.cargaProgDeLaGram("PossFormPars = FormPar CommaFormParsOpc.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);
            }//Fin while
            Code.cargaProgDeLaGram("CommaFormParsOpc = .");
        }
    }
    /* Type = ident [ "[" "]" ] */
    // First(Type)={ident}
    // Follow(Type)={ident}

    //static void Type() { }
    static void Type(out Struct xType) //Fran
    {                  //en const int size = 10,  ..en int[]
        Code.seleccLaProdEnLaGram(12);
        Code.cargaProgDeLaGram("Type = ident LbrackOpc.");
        if (la != Token.IDENT)  //debe venir un tipo (int por ej)
        {
            Errors.Error("espera un tipo");
            xType = Tab.noType;
        }
        else
        {   //laToken=int, en int[]                                       
            Check(Token.IDENT); //=> token=int y laToken=[,  .....token=int y laToken=size, en int size 
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true); //si viene de... yaPintado = true => no pinta nada
            
            Symbol sym = Tab.Find(token.str);  //busca int  y devuelve el Symbol p/int
                         //Busca Table y devuelve el Symbol p/Table
            if (ZZ.parser) Console.WriteLine("Tab.Find(" + token.str + ") =>" + sym.ToString() + "..."); //if (ZZ.readKey) Console.ReadKey();

            if (sym == null)
            {
                Errors.Error("debe venir un tipo");//Fran
                xType = Tab.noType;
            }
            else
            {
                xType = sym.type; //Devuelve int como tipo (Struct), no como nodo Symbol 
            };

                //??Tab.Insert(Symbol.Kinds.Type, token.str, Tab.noType);
                //o es un tipo array (int[]) o no (int, unaClase)

            Code.seleccLaProdEnLaGram(13);
            //Code.cargaProgDeLaGram("LbrackOpc = .| '[' ']'.");

            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false); //un "[" o lo que sigue al type (un ident en int ident1)
            if (la == Token.LBRACK)  // 
                {//int[]
                    Code.cargaProgDeLaGram("LbrackOpc = '[' ']'.");
                    Check(Token.LBRACK);
                    Check(Token.RBRACK);                  //int tipo del elem del array
                    xType = new Struct(Struct.Kinds.Arr, sym.type);
                                       //podria haber sido xType (Struct del int) en vez de sym.type  
                    //el nuevo xType que obtiene es un array de int
                }
            else Code.cargaProgDeLaGram("LbrackOpc = .");
        }
    } 
    
    /*	Statement
     *	= Designator ( "=" Expr | "(" [ ActPars ] ")" | "++" | "--" ) ";"
     *	| "if" "(" Condition ")" Statement [ "else" Statement ] 
     *	| "while" "(" Condition ")" Statement 
     *  | "do" Statement "while" "(" Condition ")" ";"
     *	| "continue" ";"
     *	| "return" [ Expr ] ";"
     *	| "read" "(" Designator ")" ";"
     *	| "write" "(" Expr [ comma number ] ")" ";"
     *	| Block
     *	| ";" .
     *	= Designator ( "=" Expr | ActParList | "++" | "--" ) ";"
     *  | ...
     */
    // First(Statement)={ident, if, while, break, return, read, write, "{", ;}
    // Follow(Statement)={else,  "}"}

    static void markLabelMio(int nroInstrParaRectificarElIf)
    {
        //agregar una nop
        Parser.nroDeInstrCorriente++;
        Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.nop;
        //Parser.cil[Parser.nroDeInstrCorriente].instrString =
        Code.cargaInstr("nop   ");

        //ir a la linea correspondiente y marcar el label con la instr corriente
        Parser.cil[nroInstrParaRectificarElIf].nroLinea = nroDeInstrCorriente;//Br cond ahora está definido
        Parser.cil[nroInstrParaRectificarElIf].instrString =
            Parser.cil[nroInstrParaRectificarElIf].instrString.Substring(0,
                Parser.cil[nroInstrParaRectificarElIf].instrString.Length - 2)
                + nroDeInstrCorriente.ToString();
        //reescribe cil (nuevoRichTexBox3) rectificando la marca
        string newRichTexBox3 = cil[0].instrString;
        for (int i = 1; i <= nroDeInstrCorriente; i++)
        {
            newRichTexBox3 = newRichTexBox3 + "\n" + cil[i].instrString;
        }
        Program1.form1.richTextBox3.Text = newRichTexBox3;
    }

    static void Statement(System.Windows.Forms.TreeNode padre)
    {//void Main() int x,i; { => val = new Table;....}
        if (ZZ.ParserStatem) Console.WriteLine("Comienza statement:" + laToken.str);
            //if (ZZ.readKey) Console.ReadKey();
        if (la == Token.IDENT) // First de Designator /// 
        {
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true); //laToken (ident)  "writeln"  ya pintado  o "var1" en var1 = 10;
            //if (muestraProducciones) MessageBoxCon3Preg();
            Item itemIzq, itemDer; // = new Item();  // 
            Code.cargaProgDeLaGram("Statement = Designator RestOfStatement");
            Code.seleccLaProdEnLaGram(31);  //scroll needed
            Code.cargaProgDeLaGram("Designator = ident  opcRestOfDesignator.");
            ///////////////
            System.Windows.Forms.TreeNode designator = new System.Windows.Forms.TreeNode("Designator = ident  opcRestOfDesignator.");
            padre.Nodes.Add(designator);
            /////////////
            Designator(out itemIzq,designator); //val   en statement
            
            String parteFinalDelDesign = token.str;
            ///////////////
            System.Windows.Forms.TreeNode RestOfstatement = new System.Windows.Forms.TreeNode("RestOfStatement = '=' Expr ';' | '('')'';'| '++'';'| '--'';' ");
            padre.Nodes.Add(RestOfstatement);
            /////////////
            if (ZZ.parser) Console.WriteLine("pasa el Designator()"); 
            switch (la)
            {
                case Token.ASSIGN:  //asignacion  Designator = ....
                    Check(Token.ASSIGN);
                    Code.cargaProgDeLaGram("RestOfStatement = '=' Expr.");
                    Code.cargaProgDeLaGram("Expr = OpcMinus Term OpcAddopTerm.");

                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true); //("=")
                    ////////////////
                    RestOfstatement.Nodes.Add("=");
                    System.Windows.Forms.TreeNode restofhijo2 = new System.Windows.Forms.TreeNode("Expr = OpcMinus Term OpcAddopTerm");
                    RestOfstatement.Nodes.Add(restofhijo2);
                    ///////////////
                    if (muestraProducciones) MessageBoxCon3Preg();

                    Expr(out itemDer,restofhijo2);
                    Code.Load(itemDer);  //... load expr ...
                    Code.Assign(itemIzq, itemDer);
                    //stsfld  globalVar  (metad para val)

                    if (ZZ.parser)
                    {
                        Console.WriteLine("Termina statement de asign: ..." + parteFinalDelDesign 
                            + " = ....." + Token.names[token.kind]+ " str=" +token.str);
                        if (ZZ.readKey) Console.ReadKey();
                    };
                    break;
                case Token.LPAR:   //Designator(....  metodo(.....
                    Check(Token.LPAR);
                    if (la == Token.MINUS || la == Token.IDENT ||
                        la == Token.NUMBER || la == Token.CHARCONST ||
                        la == Token.NEW || la == Token.LPAR)
                        ActPars();
                    Check(Token.RPAR);
                    break;
                case Token.PPLUS: // Designator++   var++
                    Check(Token.PPLUS); 
                    ///////////
                    RestOfstatement.Nodes.Add("'++'");
                    //////////
                    if (ZZ.parser) Console.WriteLine("reconoció el ++"); //zzzzzz
                    //Code.il.Emit(Code..........
                    
                    break;
                case Token.MMINUS: // var--
                    Check(Token.MMINUS);
                    ///////////
                    RestOfstatement.Nodes.Add("'--'");
                    //////////
                    break;
            }
            Check(Token.SEMICOLON);//falta nodo aca//
        }
        else
        {
            Item item, itemAncho;
            switch (la)
            {
                case Token.IF:
                    int nroInstrParaRectificarElIf; 
                    Item x; Label end;
                    
                    Check(Token.IF);
                    //////////////
                    System.Windows.Forms.TreeNode hijo3 = new System.Windows.Forms.TreeNode("'if'");
                    padre.Nodes.Add(hijo3);
                    //////////////
                    Check(Token.LPAR);
                    /////////////
                    hijo3.Nodes.Add("(");
                   
                    //////////////
                    Condition(out x);
                    /////////////
                    System.Windows.Forms.TreeNode hijo1d3 = new System.Windows.Forms.TreeNode("Condición");
                    hijo3.Nodes.Add(hijo1d3);
                    ////////////
                    if (ZZ.parser) Console.WriteLine("reconoció la cond del if");
                    Check(Token.RPAR);
                    /////////
                    hijo3.Nodes.Add(")");
                    ////////
                    Code.FJump(x);  //dentro del if (i<10) 
                    //Code.FJump(x) => bge x.fLabel, i.e. si >= debe ir a x.fLabel (debe saltar el then)
                    //x.fLabel está indefinido, luego habrá un  //luego habrà un Code.il.MarkLabel(x.fLabel);
                   
                    //Code.FJump(x)=> Parser.cil[Parser.nroDeInstrCorriente]...=> bge -1 (por ej) 
                    nroInstrParaRectificarElIf = Parser.nroDeInstrCorriente;
                    //luego habrà un Parser.cil[nroInstrParaRectificarElIf].nroLinea = label...
                    //reemplazando "bge -1" con "bge label..."
                    //////////
                    System.Windows.Forms.TreeNode hijo2d3 = new System.Windows.Forms.TreeNode("Statement = Designator RestOfStatement|'if'|'while'|'write'|'writeln'");
                    hijo3.Nodes.Add(hijo2d3);
                    /////////
                    Statement(hijo2d3); //en el if de una statement
                    if (ZZ.parser) Console.WriteLine("reconoció la Statem del if");
                    if (la == Token.ELSE)
                    {
                        Check(Token.ELSE);
                        ///////////
                        System.Windows.Forms.TreeNode hijo3d3 = new System.Windows.Forms.TreeNode("Else");
                        hijo3.Nodes.Add(hijo3d3);
                        //////////
                        end = Code.il.DefineLabel();
                        int endMio = -1;
                        Code.Jump(end);  //=> il.Emit(BR, end); 
                        Parser.nroDeInstrCorriente++;
                        Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.branchInc;
                        Parser.cil[Parser.nroDeInstrCorriente].nroLinea = endMio;  //Br Incond está indefinido
                        Code.cargaInstr("br" + "  " + Parser.cil[Parser.nroDeInstrCorriente].nroLinea.ToString());
                        //br end => br -1
                        int nroInstrParaRectificarElEndDelIf = Parser.nroDeInstrCorriente;

                        Code.il.MarkLabel(x.fLabel);  
                        markLabelMio(nroInstrParaRectificarElIf);//agregar una nop
                        //más: ir a la linea correspondiente y marcar el label con la instr corriente
                        //más: reescribe cil (nuevoRichTexBox3) rectificando la marca

                        Statement(hijo3d3);  //en el else de una statemnet
                        if (ZZ.parser) Console.WriteLine("reconoció la Statem del else del if");
                        Code.il.MarkLabel(end);
                        markLabelMio(nroInstrParaRectificarElEndDelIf);//agregar una nop
                        //más: ir a la linea correspondiente y marcar el label con la instr corriente
                        //más: reescribe cil (nuevoRichTexBox3) rectificando la marca
                    }
                    else
                    {
                        Code.il.MarkLabel(x.fLabel);
                        markLabelMio(nroInstrParaRectificarElIf);//agregar una nop
                        //más: ir a la linea correspondiente y marcar el label con la instr corriente
                        //más: reescribe cil (nuevoRichTexBox3) rectificando la marca
                    }
                    break;
                
                case Token.WHILE:
                    //Item x;
                    int nroInstrParaRectificarElWhile; 
                    Check(Token.WHILE);
                    ////////////
                    System.Windows.Forms.TreeNode hijo33 = new System.Windows.Forms.TreeNode("'While'");
                    padre.Nodes.Add(hijo33);
                    ////////////
                    Label top = Code.il.DefineLabel();
                    int topMio = -1;

                    Code.il.MarkLabel(top); 
                    topMio = Parser.nroDeInstrCorriente +1 ; //(instr sig a la actual)
 
                    //para luego..Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.branchInc;
                    //Parser.cil[Parser.nroDeInstrCorriente].nroLinea = topMio;  //Br Incond está definido
                    
                    Check(Token.LPAR);  
                    Condition(out x);
                    ////////////
                    hijo33.Nodes.Add("'('");
                    hijo33.Nodes.Add("Condición");
                  
                    ////////////
                    if (ZZ.parser) { Console.WriteLine("Termina con la cond del while");  };
                    Check(Token.RPAR);
                    hijo33.Nodes.Add("')'");
                    //////////
                    Code.FJump(x); //dentro del while (i<10) 
                    //Code.FJump(x) => bge x.fLabel, i.e. si >= debe ir a x.fLabel (debe salir del loop)
                    //x.fLabel está indefinido, luego habrá un  //luego habrà un Code.il.MarkLabel(x.fLabel);

                    //Code.FJump(x)=> Parser.cil[Parser.nroDeInstrCorriente]...=> bge -1 (por ej) 
                    nroInstrParaRectificarElWhile = Parser.nroDeInstrCorriente;
                    //luego habrà un Parser.cil[nroInstrParaRectificarElWhile].nroLinea = label...
                    //reemplazando "bge -1" con "bge label..."

                    Check(Token.LBRACE);
                    ////////////////////////
                    hijo33.Nodes.Add("'{'");
                    
                    ///////////////////////
                    //cuerpo del while  
                    while (la != Token.RBRACE && la != Token.EOF) {
                        System.Windows.Forms.TreeNode Statement1 = new System.Windows.Forms.TreeNode("Statement = Designator RestOfStatement|'if'|'while'|'write'|'writeln'");
                        hijo33.Nodes.Add(Statement1); 
                        Statement(Statement1);
                    } //dentro del while

                    Check(Token.RBRACE);
                    //////////////
                    hijo33.Nodes.Add("'}'");
                    /////////////
                    if (ZZ.parser) { Console.WriteLine("Termina statement while"); if (ZZ.readKey) Console.ReadKey(); };
                    Code.Jump(top);
                    Parser.nroDeInstrCorriente++;
                    Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.branchInc;
                    Parser.cil[Parser.nroDeInstrCorriente].nroLinea = topMio;  //Br Incond está definido
                    Code.cargaInstr("br" + "  " + Parser.cil[Parser.nroDeInstrCorriente].nroLinea.ToString());
                                                                                 //br top
                    Code.il.MarkLabel(x.fLabel);
                    //agregar una nop
                    Parser.nroDeInstrCorriente++;
                    Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.nop;
                    //Parser.cil[Parser.nroDeInstrCorriente].instrString =
                    Code.cargaInstr("nop   ");

                    //ir a la linea correspondiente y marcar el label con la instr corriente
                    Parser.cil[nroInstrParaRectificarElWhile].nroLinea = nroDeInstrCorriente;//Br cond ahora está definido
                    Parser.cil[nroInstrParaRectificarElWhile].instrString =
                        Parser.cil[nroInstrParaRectificarElWhile].instrString.Substring(0,
                            Parser.cil[nroInstrParaRectificarElWhile].instrString.Length - 2)
                            + nroDeInstrCorriente.ToString();

                    Program1.form1.richTextBox3.Text = "";

                    //System.Windows.Forms.MessageBox.Show("empieza......");
                    //reescribe cil (nuevoRichTexBox3) rectificando la marca
                    string nuevoRichTexBox3 = cil[0].instrString;  
                    for (int i = 1; i <= nroDeInstrCorriente; i++)
                      {
                          nuevoRichTexBox3 = nuevoRichTexBox3 + "\n" + cil[i].instrString;
                      }
                    Program1.form1.richTextBox3.Text = nuevoRichTexBox3;
                    break;
                case Token.BREAK://///preguntat///////
                    Check(Token.BREAK);
                    Check(Token.SEMICOLON);
                    break;
                case Token.RETURN://///preguntat///////
                    Check(Token.RETURN);
                    // First(Expr)
                    if (la == Token.MINUS || la == Token.IDENT || la == Token.NUMBER ||
                        la == Token.CHARCONST || la == Token.NEW || la == Token.LPAR)
                        Expr(out item,null);//Falta procesar
                    Check(Token.SEMICOLON);
                    break;
                case Token.READ: /////preguntat///////
                    Check(Token.READ);
                    Check(Token.LPAR);
                    if (la == Token.IDENT)
                        Designator(out item,null);//En el READ
                    Check(Token.RPAR);
                    Check(Token.SEMICOLON); 
                    //ZZ.parser = true; 
                    if (ZZ.parser) Console.WriteLine("reconoció el Read");
                    break;


                case Token.WRITELN: ///  // en statement
                    //token queda con ";" y laToken = WRITELN y ch con '('               

                    Check(Token.WRITELN);
                    ///////////////////////
                    System.Windows.Forms.TreeNode hijowriteln = new System.Windows.Forms.TreeNode("writeln");
                    padre.Nodes.Add(hijowriteln);
                    ///////////////////////
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true); //ya lo pintó
                    //token queda con WRITELN y laToken =  "(" y ch con Comm Doble

                    Code.cargaProgDeLaGram("Statement = writeln '(' argString ')' ';' ");
                        //                           Designator RestOfStatement	
                    //    |	"if" "(" Condition ")" Statement ElseOpc
                    //    |	"while" "(" Condition ")" Statement
                    //    |	"break" ";"
                    //    |	"return" [ Expr ] ";"
                    //    |	"read" "(" Designator ")" ";"
                    //    |	"write" "(" Expr [ "," number ] ")" ";"
                    //    |	"writeln" "("  argString ")" ";"

                    //equivalente a Check(Token.LPAR);
                    //debe quedar token = "("  y laToken = "texto posiblem vacio"               
                    if (la == Token.LPAR) //ch = Comm Doble
                     {
                         Code.coloreaConRojo("latoken");
                         //Code.coloreaConRojo(false); //pinta el "("
                         Code.coloreaConRojo("token");
                         //Code.coloreaConRojo(true); //solo para que deje yaPintado en false
                         //Scan(); //token = "("
                         hijowriteln.Nodes.Add("'('");
                        if (Scanner.ch != '\"') 
                            Errors.Error("Se esperaba una COMILLA DOBLE");
                        else
                        {
                            string argStr = "";
                            Scanner.NextCh(); //ch = 2º com doble o Primer char del argStr

                            while (Scanner.ch != '\"')  
                             {
                                //if (ch == EOF) return new Token(Token.EOF, line, col);
                                argStr = argStr + Scanner.ch.ToString();
                                Scanner.NextCh();  //ch = 2º char del argStr o Com Doble
                             }
                            //ch = Com Doble
                            token = new Token(Scanner.line, Scanner.col);
                            token.kind = Token.COMILLADOBLE; //se va a llamar argDeWriteLine
                            token.str = argStr; // excluye las comillas dobles
                            //token.line lo deja =
                            token.col = token.col - argStr.Length; //+ 1; // -3; //DESPUES DEL "("

                            Code.coloreaConRojo("token");
                            //Code.coloreaConRojo(true);


                            Parser.nroDeInstrCorriente++;
                            //Agregar  ldString  argStr, 
                            Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.ldstr;
                            Parser.cil[Parser.nroDeInstrCorriente].argDelWriteLine = argStr;
                            Code.cargaInstr("ldstr \"" + argStr + "\" ");

                            Parser.nroDeInstrCorriente++;
                            Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.writeln;
                            Code.cargaInstr("call writeln#(string)");
                            //Parser.cil[Parser.nroDeInstrCorriente].nro = item.val; // item.val;           //item.val;  aaaaaaa 
                            //Parser.cil[Parser.nroDeInstrCorriente].argDelWriteLine = argStr;  //Provisorio ya no se usa argDelWriteLine


                            Scanner.NextCh(); //ch=")"
                            token = laToken; //token queda con argDeWriteLine
                            laToken = Scanner.Next(); //la token queda con ")"
                            la = laToken.kind;
                            Code.il.EmitWriteLine(argStr);
                            Check(Token.RPAR);
                            Code.coloreaConRojo("token");
                            //Code.coloreaConRojo(true);
                            //////////////////////////////
                            hijowriteln.Nodes.Add("'argString'"); 
                            hijowriteln.Nodes.Add("')'");
                            /////////////////////////////
                                               //+ Code.blancos(22-11-argStr.Length));
                            Check(Token.SEMICOLON);
                            Code.coloreaConRojo("token");
                            //Code.coloreaConRojo(true);
                            //////////////////////
                            hijowriteln.Nodes.Add("';'");
                            /////////////////////
                        }
                     }    

                    break;
                case Token.WRITE: ///
                    
                    Check(Token.WRITE);
                    //////////////
                    System.Windows.Forms.TreeNode hijowrite = new System.Windows.Forms.TreeNode("'Write'");
                    padre.Nodes.Add(hijowrite);
                    //////////////
                    Check(Token.LPAR);
                    //////////
                    hijowrite.Nodes.Add("'('");
                    System.Windows.Forms.TreeNode hijodehijowrite = new System.Windows.Forms.TreeNode("Expr = OpcMinus Term OpcAddopTerm");
                    hijowrite.Nodes.Add(hijodehijowrite);
                    ////////
                    // First(Expr)
                    if (la == Token.MINUS || la == Token.IDENT || la == Token.NUMBER ||
                        la == Token.CHARCONST || la == Token.NEW || la == Token.LPAR)
                    //Code.il.EmitWriteLine("dentro del WRITE, antes del Expr(out item)");

                    AA(out item); ///?????
                    Expr(out item,hijodehijowrite);
                    //System.Windows.Forms.MessageBox.Show("en el write item.val.... " + item.val.ToString());

                    //System.Windows.Forms.MessageBox.Show("luegod e expr item.val.... " + item.val.ToString());
                    Check(Token.COMMA);
                    /////////////
                    hijowrite.Nodes.Add("','");
                    System.Windows.Forms.TreeNode hijo2dehijowrite = new System.Windows.Forms.TreeNode("Expr = OpcMinus Term OpcAddopTerm");
                    hijowrite.Nodes.Add(hijo2dehijowrite);
                    ////////////
                    Expr(out itemAncho,hijo2dehijowrite);
                    
                    //Code.il.EmitWriteLine("......dentro del WRITE!!! iujuuuu ...");
                    //Code.il.EmitCall(OpCodes.Call, Code.writeLineMI, null);
                     //Code.il.Emit(Code.LDC7);
                    // Code.il.EmitWriteLine("X = "); 
                    
                    Code.il.EmitCall(Code.CALL, Code.writeInt, null); //Probar   provisorio

                    Parser.nroDeInstrCorriente++;
                    Parser.cil[Parser.nroDeInstrCorriente].accionInstr = Parser.AccionInstr.write;
                    Parser.cil[Parser.nroDeInstrCorriente].nro = item.val; // item.val;           //item.val;  aaaaaaa 
                    
                    Code.cargaInstr("call write#(int32,int32)");

                    //Code.il.EmitWriteLine("\nReadKeyMio()"); Probar, no se si funca despues
                    //ReadKeyMio();  //no funca en combinacion con Code.readInt

                    //Console.WriteLine(" Primero sale este cartel, mucho despues aparecerá el write del CIL");
                    //Code.il.EmitWriteLine("The .............................value of 'x' is:"); 
                    //Code.il.EmitWriteLine(1000);

                    Check(Token.RPAR);
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    //////
                    hijowrite.Nodes.Add("')'");
                    //////
                    Check(Token.SEMICOLON);
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    //////
                    hijowrite.Nodes.Add("';'");
                    //////
                    break;
                case Token.LBRACE:
                    System.Windows.Forms.TreeNode hijoblock = new System.Windows.Forms.TreeNode("Block = '{' Statement '}'");
                    padre.Nodes.Add(hijoblock);
                    Block(hijoblock);  //bloque dentro de una sentencia
                    break;
                case Token.SEMICOLON:
                    Check(Token.SEMICOLON); padre.Nodes.Add("';'");
                    break;
                default:
                    Errors.Error("espero una sentencia");
                    break;
            };
        };
        //if (ZZ.parser)  Console.WriteLine("TERMINA statement");
    }

	/* Block = "{" { Statement } "}" . */
    // First(Block)={"{"}; Follow(Block)={else, "{"}
    static void Block(System.Windows.Forms.TreeNode padre)
    {////void Main() int x,i;     => {val = new Table;....}
        ///////
        System.Windows.Forms.TreeNode hijomayor = new System.Windows.Forms.TreeNode("Block = '{'  StatementsOpc '}'.");
        padre.Nodes.Add(hijomayor);
        //////
        Check(Token.LBRACE);
        /////
        hijomayor.Nodes.Add("{");
        //////
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);  //Ya lo pintó
        Code.seleccLaProdEnLaGram(16);
        Code.cargaProgDeLaGram("Block = '{'  StatementsOpc '}'.");
        Code.seleccLaProdEnLaGram(17); //StatementsOpc = . | Statement StatementsOpc.
   
        int ii = 1;
        while (la != Token.RBRACE)
        {
            if ((la == Token.IDENT || la == Token.IF || la == Token.WHILE || la == Token.BREAK
              || la == Token.RETURN || la == Token.READ || la == Token.WRITE || la == Token.WRITELN
              || la == Token.LBRACE || la == Token.SEMICOLON) && la != Token.EOF)
            {
                Code.coloreaConRojo("latoken");
                //Code.coloreaConRojo(false);  //pinta el primer token de la sentencia (writeln por ej)
                Code.cargaProgDeLaGram("StatementsOpc = Statement StatementsOpc.");
                Code.seleccLaProdEnLaGram(18);
                ///////
                System.Windows.Forms.TreeNode hijomayor2 = new System.Windows.Forms.TreeNode("StatementsOpc = Statement StatementsOpc.");
                hijomayor.Nodes.Add(hijomayor2);
                System.Windows.Forms.TreeNode hijomenor = new System.Windows.Forms.TreeNode("Statement = Designator RestOfStatement|'if'|'while'|'write'|'writeln'");
                hijomayor2.Nodes.Add(hijomenor);
                ///////

                    //18. Statement	=	
                    //                           Designator RestOfStatement	
                    //    |	"if" "(" Condition ")" Statement ElseOpc
                    //    |	"while" "(" Condition ")" Statement
                    //    |	"break" ";"
                    //    |	"return" [ Expr ] ";"
                    //    |	"read" "(" Designator ")" ";"
                    //    |	"write" "(" Expr [ "," number ] ")" ";"
                    //    |	Block
                    //    |	";".

             

                if (ZZ.ParserStatem)
                {
                    Console.WriteLine(".......Comienza statement nro:");
                    Console.Write(ii); Console.WriteLine("->" + laToken.str);
                }

                Statement(hijomenor);  //dentro de block()

            }//Fin if 
            else {token.line = Scanner.line; token.col = Scanner.col -1;
                  token.str = "?";
                  Errors.Error("espero una sentencia"); 
                 }
          ii++;
          Code.seleccLaProdEnLaGram(17); //StatementsOpc = . | Statement StatementsOpc.
        }//Fin while
        Code.cargaProgDeLaGram("StatementsOpc = . "); 
        
        Check(Token.RBRACE);
        /////////////////
        hijomayor.Nodes.Add("'}'");
        ////////////////
        Code.cargaProgDeLaGram("StatementsOpc = .");
        Code.coloreaConRojo("token");
        //Code.coloreaConRojo(true);  //ya habia pintado el ")", ahora pinta "}"
        if (Parser.muestraProducciones)
                System.Windows.Forms.MessageBox.Show("Termina bloque ");

        // el cierre del bloque no está aquí
                        //Statement();
                //Symbol sym = Tab.Find("Table");
                //sym.ctor = sym.type.sysTye.   DefineConstructor(MethodAttributes.Public,
                //                   CallingConventions.Standard, new Type[0]);
                //ConstructorInfo ci = typeof(Int16).GetConstructor(
                //     new[] { typeof(string), typeof(int) });

                //ESTA ES LA PARTE QUE NO FUNCIONA
                ///inner1 = module.DefineType(sym.name, INNERATTR);
                //define default contructor (calls base constructor)
                //ConstructorBuilder ctor = inner1.DefineConstructor(MethodAttributes.Public,  //rr
                //                               CallingConventions.Standard, new Type[0]);
                //Code.il.Emit(Code.POP);


                //ConstructorInfo ci = typeof(System.Int32).GetConstructor(new Type[0]);
                //Code.il.Emit(Code.NEWOBJ, ci); //sym.ctor;
                //Code.il.Emit(Code.POP);
                //Code.il.Emit(Code.STLOC0);
                //Code.il.Emit(Code.LDC7);
                //Code.il.Emit(Code.STLOC1);

                // Code.il.EmitWriteLine("...  dentro del Main...");

                //Otro modo para el writeLine
                //Type[] wlParams = new Type[] { typeof(int) };
                //MethodInfo writeLineMI =
                //    typeof(Console).GetMethod("WriteLine", wlParams);
                //////Metadata p/el metodo
                ////// Console.WriteLine(int)
                ////                Code.il.Emit(Code.LDC3);
                ////                Code.il.EmitCall(OpCodes.Call, writeLineMI, null);

                ////               ReadKeyMio();



                ////MethodInfo readIntMI =
                ////    typeof(Console).GetMethod("WriteLine", wlParams);

                ////// Call the System.Console.Read function
                //// call int32 [mscorlib]System.Console::Read()

                //Console.WriteLine("Comienza ret, create type, createInst, Invoke Main"); if (ZZ.readKey) Console.ReadKey();
                //Code.il.Emit(Code.RET);  //si lo saco se clava en el InvokeMember
                //Type ptType1 = Code.program.CreateType();
                //object ptInstance1 =
                //        Activator.CreateInstance(ptType1, new object[0]);  //new object[0] si sin parms
                //ptType1.InvokeMember("Main", BindingFlags.InvokeMethod, null, ptInstance1, new object[0]);
                //Console.WriteLine("\nTermina ret, create type, createInst, Invoke Main"); if (ZZ.readKey) Console.ReadKey();

    }//Fin Block

	/* ActParList = "(" [ ActPars ] ")" . */
 /*   static void ActParList()
    {
        Check(Token.LPAR);
        if (la == Token.MINUS || la == Token.IDENT || la == Token.NUMBER || la == Token.CHARCONST ||
            la == Token.NEW || la == Token.LPAR)
            ActPars();
        Check(Token.RPAR);
    }
    */

	/* ActPars = Expr { "," Expr } . */
    // First(ActPars)={-,ident, number, charconst, new, "("}
    // Follow(ActPars)={")"}
    static void ActPars()
    {
        Item item; 
        Expr(out item,null);
        while (la == Token.COMMA && la != Token.EOF)
        {
            Check(Token.COMMA);
            Expr(out item,null);
        }
    }

	/* Condition = CondTerm { "||" CondTerm } . */
    // First(Condition)={-, ident, number, charconst, new, "("}
    // Follow(Condition)={")"}
    static void Condition(out Item x)
    {
        Item y;
        CondTerm(out x);
        while (la == Token.OR && la != Token.EOF)
        {
            Check(Token.OR);
            Code.TJump(x); 
    		Code.il.MarkLabel(x.fLabel); 
	    	CondTerm(out y);
            x.relop = y.relop; x.fLabel = y.fLabel; 
        }
    }//Fin static void Condition(out Item x)

    static void Condition(System.Windows.Forms.TreeNode padre)
    {
        //Item y;
        /////////
        System.Windows.Forms.TreeNode hijo = new System.Windows.Forms.TreeNode("Término");
        padre.Nodes.Add(hijo);
        ////////
        CondTerm(hijo);
        while (la == Token.OR && la != Token.EOF)
        {
            Check(Token.OR);
            //Code.TJump(x);
            //Code.il.MarkLabel(x.fLabel);
            System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("Término");
            padre.Nodes.Add(hijo1);
            CondTerm(hijo1);
            //x.relop = y.relop; x.fLabel = y.fLabel;
        }
    }//Fin static void Condition(out Item x)

    
    /* CondTerm = CondFact { "&&" CondFact } . */
    // First(CondTerm)={-, ident, number, charconst, new, "("}
    // Follow(CondTerm)={"||"} 

   
    static void CondTerm(out Item x)
    {
        Item y;
        CondFact(out x);
        while (la == Token.AND && la != Token.EOF)
        {
            Check(Token.AND);
            Code.FJump(x);
            CondFact(out y); 
            x.relop = y.relop; x.tLabel = y.tLabel; 
        }
	}//Fin static void CondTerm(out Item x)
    
    
    
    static void CondTerm(System.Windows.Forms.TreeNode padre)
    {
        System.Windows.Forms.TreeNode hijo = new System.Windows.Forms.TreeNode("Factor");
        padre.Nodes.Add(hijo);
        CondFact(hijo);
        while (la == Token.AND && la != Token.EOF)
        {
            Check(Token.AND);
            System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("Factor");
            padre.Nodes.Add(hijo1);
            CondFact(hijo1);
        }
    }
    
	/* CondFact = Expr Relop Expr . */
    // First(CondFact)={-, ident, number, charconst, new, "("}
    // Follow(CondFact)={"&&"}
    static void Expr(out Item item)
    {
        OpCode op; Item itemSig;

        if (la == Token.MINUS)
        {
            Check(Token.MINUS);
            //falta Code.cargaProgDeLaGram(".....
            Term(out item);  //
            if (item.type != Tab.intType) Errors.Error("Operando debe ser de tipo int");
            if (item.kind == Item.Kinds.Const) item.val = -item.val;
            else
            {
                Code.Load(item); Code.il.Emit(Code.NEG);
            };
        }
        else
        {
            Code.cargaProgDeLaGram("OpcMinus = .");
            Code.cargaProgDeLaGram("Term = Factor  OpcMulopFactor.");
            Code.cargaProgDeLaGram("Factor = Designator  OpcRestOfMethCall.");
            Code.cargaProgDeLaGram("Designator = ident opcRestOfDesignator.");
            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false);//1º parte de Term (y de Factor), por ej 123
            if (muestraProducciones) MessageBoxCon3Preg();
            Term(out item);
        }
        //Console.WriteLine("la=", Token.names[la]); if (ZZ.readKey) Console.ReadKey();  Por que no anda?
        string opString = "";
        while ((la == Token.PLUS || la == Token.MINUS) && la != Token.EOF)
        {
            Code.cargaProgDeLaGram("OpcAddopTerm = Addop Term.");
            if (la == Token.PLUS)
            {
                Scan(); op = Code.ADD; opString = "add       ";

                Code.cargaProgDeLaGram("Addop = '+'.");
                Code.cargaProgDeLaGram("Term = Factor OpcMulopFactor.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);
                if (muestraProducciones) MessageBoxCon3Preg();
            }
            else if (la == Token.MINUS)
            {
                Scan(); op = Code.SUB; opString = "sub       "; Code.cargaProgDeLaGram("Addop = '-'.");

                Code.cargaProgDeLaGram("Addop = '-'.");
                Code.cargaProgDeLaGram("Term = Factor OpcMulopFactor.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);
                if (muestraProducciones) MessageBoxCon3Preg();
                //cil[nroDeInstrCorriente].accionInstr = AccionInstr.add;
            }
            else op = Code.DUP; //nunca entra por acá, solo p/q no de error
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);

            Code.Load(item);
            Term(out itemSig);
            Code.Load(itemSig);
            if (item.type != Tab.intType || itemSig.type != Tab.intType)
                Errors.Error("Los operandos deben ser de tipo int");

            nroDeInstrCorriente++;
            Code.il.Emit(op);
            Code.cargaInstr(opString);
            if (op == Code.ADD)
                cil[nroDeInstrCorriente].accionInstr = AccionInstr.add;
            else if (op == Code.SUB)
                cil[nroDeInstrCorriente].accionInstr = AccionInstr.sub;
            else
                System.Windows.Forms.MessageBox.Show("aun no implementado 343323");
        }//Fin While
        //System.Windows.Forms.MessageBox.Show("en expr item.val.... " + item.val.ToString());

        // item = new Item(12345);
    }//Fin Expr
    static void CondFact(out Item x)
    {
        Item y; int op;
        Expr(out x); Code.Load(x); 
        Relop(out op); 
        Expr(out y); Code.Load(y);
        if (!x.type.CompatibleWith(y.type))
                Errors.Error("type mismatch");
            else if (x.type.IsRefType() && 
                    op!=Token.EQ && op!=Token.NE)
                  Errors.Error("only equality checks ...");
            x = new Item(op, x.type);
    }
    
    
    static void CondFact(System.Windows.Forms.TreeNode padre)  //old
    {
        ////////////
        System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("Expresión");
        padre.Nodes.Add(hijo1);
        ////////////
        Item item1, item2;
        Expr(out item1,hijo1);
        //////////
        System.Windows.Forms.TreeNode hijo2 = new System.Windows.Forms.TreeNode("Condición");
        padre.Nodes.Add(hijo2);
        //////////
        Relop(hijo2);
        //////////
        System.Windows.Forms.TreeNode hijo3 = new System.Windows.Forms.TreeNode("Expresión");
        padre.Nodes.Add(hijo3);
        //////////
        Expr(out item2,hijo3);
    }

	/* Expr = [ "-" ] Term { Addop Term } . 
	 *      = ( "-" Term | Term ) { Addop Term } .
	 */
    // First(Expr)={-, ident, number, charconst, new, "("}
    // Follow(Expr)={",", ";", "==", >, >=, <, <=, ")", "]"}
    static void AA(out Item item)
    {
        item = new Item(12345);
    }

    static void Expr(out Item item, System.Windows.Forms.TreeNode padre)
    {
        OpCode op; Item itemSig;

        if (la == Token.MINUS)
        {
            Check(Token.MINUS);
            ///////
            System.Windows.Forms.TreeNode hijominus = new System.Windows.Forms.TreeNode("OpcMinus= .|'-'");
            hijominus.Nodes.Add("OpcMinus= '-'");
            padre.Nodes.Add(hijominus);
            System.Windows.Forms.TreeNode hijoterm = new System.Windows.Forms.TreeNode("Term = Factor OpcMulopFactor");
            padre.Nodes.Add(hijoterm);
            ///////
            //falta Code.cargaProgDeLaGram(".....
            Term(out item,hijoterm);  //
            if (item.type != Tab.intType) Errors.Error("Operando debe ser de tipo int");
            if (item.kind == Item.Kinds.Const) item.val = -item.val;
            else
            {
                Code.Load(item); Code.il.Emit(Code.NEG);
            };
        }
        else
        {
            Code.cargaProgDeLaGram("OpcMinus = .");
            Code.cargaProgDeLaGram("Term = Factor  OpcMulopFactor.");
            Code.cargaProgDeLaGram("Factor = Designator  OpcRestOfMethCall.");
            Code.cargaProgDeLaGram("Designator = ident opcRestOfDesignator.");
            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false);//1º parte de Term (y de Factor), por ej 123
            ///////
            System.Windows.Forms.TreeNode hijominus = new System.Windows.Forms.TreeNode("OpcMinus= .|'-'");
            hijominus.Nodes.Add("OpcMinus= .");
            padre.Nodes.Add(hijominus);
            System.Windows.Forms.TreeNode hijoterm = new System.Windows.Forms.TreeNode("Term = Factor OpcMulopFactor");
            padre.Nodes.Add(hijoterm);
            ///////
            if (muestraProducciones) MessageBoxCon3Preg();
            Term(out item,hijoterm);
        }
        //Console.WriteLine("la=", Token.names[la]); if (ZZ.readKey) Console.ReadKey();  Por que no anda?
        string opString = "";
        ////////
        System.Windows.Forms.TreeNode hijoOpcAddopTerm = new System.Windows.Forms.TreeNode("OpcAddopTerm = . |Addop Term.");
        padre.Nodes.Add(hijoOpcAddopTerm);
        bool banderita = false;
        ///////
        while ((la == Token.PLUS || la == Token.MINUS) && la != Token.EOF)
        {
            Code.cargaProgDeLaGram("OpcAddopTerm = Addop Term.");
            ///////
            System.Windows.Forms.TreeNode hijo1 = new System.Windows.Forms.TreeNode("OpcAddopTerm = Addop Term");
            hijoOpcAddopTerm.Nodes.Add(hijo1); banderita = true;
            //////
            if (la == Token.PLUS)
            {
                Scan(); op = Code.ADD; opString = "add       ";

                Code.cargaProgDeLaGram("Addop = '+'.");
                Code.cargaProgDeLaGram("Term = Factor OpcMulopFactor.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);
                //////////////
                System.Windows.Forms.TreeNode hijito = new System.Windows.Forms.TreeNode("Addop = '+' | '-'");
                hijito.Nodes.Add("'+'");
                hijo1.Nodes.Add(hijito);
                /////////////
                if (muestraProducciones) MessageBoxCon3Preg();
            }
            else if (la == Token.MINUS)
            {
                Scan(); op = Code.SUB; opString = "sub       "; Code.cargaProgDeLaGram("Addop = '-'.");

                Code.cargaProgDeLaGram("Addop = '-'.");
                Code.cargaProgDeLaGram("Term = Factor OpcMulopFactor.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true);
                //////////////
                System.Windows.Forms.TreeNode hijito = new System.Windows.Forms.TreeNode("Addop = '+' | '-'");
                hijito.Nodes.Add("'-'");
                hijo1.Nodes.Add(hijito);
                /////////////
                if (muestraProducciones) MessageBoxCon3Preg();
                //cil[nroDeInstrCorriente].accionInstr = AccionInstr.add;
            }
            else op = Code.DUP; //nunca entra por acá, solo p/q no de error
            Code.coloreaConRojo("token");
            //Code.coloreaConRojo(true);

            Code.Load(item);
            //////////////
            System.Windows.Forms.TreeNode hijito2 = new System.Windows.Forms.TreeNode("Term = Factor OpcMulopFactor");
            hijo1.Nodes.Add(hijito2);
            /////////////
            Term(out itemSig,hijito2);
            Code.Load(itemSig);
            if (item.type != Tab.intType || itemSig.type != Tab.intType)
                Errors.Error("Los operandos deben ser de tipo int");

            nroDeInstrCorriente++;
            Code.il.Emit(op);
            Code.cargaInstr(opString);
            if (op == Code.ADD)
                cil[nroDeInstrCorriente].accionInstr = AccionInstr.add;
            else if (op == Code.SUB)
                cil[nroDeInstrCorriente].accionInstr = AccionInstr.sub;
            else
                System.Windows.Forms.MessageBox.Show("aun no implementado 343323");
        }
        if (banderita == false)
        {
            hijoOpcAddopTerm.Nodes.Add(" . ");
        }
        //Fin While
        //System.Windows.Forms.MessageBox.Show("en expr item.val.... " + item.val.ToString());

       // item = new Item(12345);
    }//Fin Expr
    static void Designator(out Item item)
    {  ////void Main() int x,i; {    => val    = new Table;....}

        //debe buscar el designator en la tabla de simbolos
        Check(Token.IDENT); //ahora token.str="val"      y laToken= "="
        //Code.coloreaConRojo(true); ya lo mostró antes
        //if (muestraProducciones) MessageBoxCon3Preg();

        Code.seleccLaProdEnLaGram(31);
        Code.cargaProgDeLaGram("Designator = ident  opcRestOfDesignator.");

        Symbol sym = Tab.Find(token.str);
        if (ZZ.ParserStatem) Console.WriteLine("token.str:" + token.str);
        if (sym == null) Errors.Error(sym.name + "..no está en la Tab");

        item = new Item(sym);
        if ((la == Token.PERIOD || la == Token.LBRACK) && la != Token.EOF)
        {
            while ((la == Token.PERIOD || la == Token.LBRACK) && la != Token.EOF)//hacer do...while
            {
                //debe seguir buscando en la Tab
                if (ZZ.parser) Console.Write("field..." + token.str + " (val)"); //val
                if (la == Token.PERIOD)
                {
                    Check(Token.PERIOD); //caso del val . pos
                    Code.cargaProgDeLaGram("opcRestOfDesignator =  '.' ident.");
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    if (muestraProducciones) MessageBoxCon3Preg();

                    Check(Token.IDENT); //pos
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    if (muestraProducciones) MessageBoxCon3Preg();

                    if (ZZ.parser) Console.WriteLine(" . " + token.str + " (pos)"); //pos
                    if (item.type.kind == Struct.Kinds.Class)
                    {
                        //falta Code.cargaProgDeLaGram("......
                        Code.Load(item);  //lleva el Item al  Stack

                        Symbol symField = Tab.FindSymbol(token.str, sym.type.fields);
                        //sim = Tab.FindSymbol(token.str, sym.type.fields);//pierde sim orig pero sirve,
                        // para luego hacer item = new Item(sym);

                        Struct xTypeField;
                        if (symField == null)
                        {
                            Errors.Error("..--debe venir un tipo");//Fran
                            xTypeField = Tab.noType;
                        }
                        else
                        {
                            if (ZZ.parser) Console.WriteLine("encuentra " + symField.name);
                            xTypeField = symField.type; //Devuelve int como tipo (Struct), no como nodo Symbol 
                        };
                        item.sym = symField; // Tab.FindField(token.str, item.type);
                        item.type = item.sym.type; //int        f     clase
                    }
                    else Errors.Error(sym.name + " is not an object");
                    item.kind = Item.Kinds.Field; //Field
                }
                else
                    if (la == Token.LBRACK)
                    {
                        Check(Token.LBRACK);
                        Code.cargaProgDeLaGram("opcRestOfDesignator =  '[' Expr ']'.");
                        Code.coloreaConRojo("token");
                        //Code.coloreaConRojo(true);
                        if (muestraProducciones) MessageBoxCon3Preg();

                        Code.Load(item);
                        Item itemSig;
                        Expr(out itemSig);

                        if (item.type.kind == Struct.Kinds.Arr)
                        {
                            if (itemSig.type != Tab.intType) Errors.Error("index must be of type int");
                            Code.Load(itemSig);  //carga el subindice en el Stack
                            itemSig.type = item.type.elemType;//Si char[10] a; => x.type quedará con char
                        }
                        else Errors.Error(sym.name + " is not an array");
                        item.kind = Item.Kinds.Elem;

                        Check(Token.RBRACK);
                    }
            }//Fin while
            //Falta Code.cargaProgDeLaGram("...
        }
        else
        {
            Code.cargaProgDeLaGram("opcRestOfDesignator =  .");
            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false); //lo que sigue al designator (por ej "=", en Designator = Expr)
            if (muestraProducciones) MessageBoxCon3Preg();

            Code.Load(item);  //(item.val ya tiene valor)

        }
    } //Fin Designator(out Item item)
    static void Factor(out Item item)
    {
        Struct xType;  //luego debe devolver el xType en Factor(...)
        if (la == Token.IDENT)
        {
            Designator(out item); //en el Factor
            if (la == Token.LPAR)
            {//meth(params)
                Check(Token.LPAR);
                Code.cargaProgDeLaGram("OpcRestOfMethCall = '(' OpcActPars ')'.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true); // el "("
                if (muestraProducciones) MessageBoxCon3Preg();
                if (la == Token.MINUS || la == Token.IDENT ||
                    la == Token.NUMBER || la == Token.CHARCONST ||
                    la == Token.NEW || la == Token.LPAR)
                    ActPars();  //Esta parte falta
                Check(Token.RPAR);
            }
            else
            {
                Code.cargaProgDeLaGram("OpcRestOfMethCall = .");
                Code.coloreaConRojo("latoken");
                //Code.coloreaConRojo(false); // el "("
                if (muestraProducciones) MessageBoxCon3Preg();
            }
        }
        else
            switch (la)
            {
                case Token.NUMBER:
                    Check(Token.NUMBER);
                    Code.cargaProgDeLaGram("Factor = number.");
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    if (muestraProducciones) MessageBoxCon3Preg();

                    item = new Item(token.val);//Nuevo
                    Code.Load(item);
                    break;
                case Token.CHARCONST:
                    Check(Token.CHARCONST);
                    item = new Item(token.val); item.type = Tab.charType;
                    break;
                case Token.NEW:
                    Check(Token.NEW);

                    Check(Token.IDENT);  //Deberia buscar en la Tab y verificar que sea un Tipo o una clase 
                    Symbol sym = Tab.Find(token.str);  //ident debe ser int, char, o una clase  (Table)
                    if (sym.kind != Symbol.Kinds.Type) Errors.Error("type expected");
                    Struct type = sym.type;
                    //si es clase, sym.type contiene un puntero a los campos de esa clase

                    if (ZZ.parser) Console.WriteLine("Tab.Find(" + token.str + ") =>" + sym.ToString() + "..."); //if (ZZ.readKey) Console.ReadKey();
                    if (sym == null)
                    {
                        Errors.Error("--debe venir un tipo");//Fran
                        xType = Tab.noType;
                    }
                    else
                    {
                        xType = sym.type; //Devuelve int como tipo (Struct), no como nodo Symbol 
                        if (ZZ.parser) Console.WriteLine("Encontró " + token.str); //if (ZZ.readKey) Console.ReadKey();
                    };
                    if (ZZ.parser) Console.WriteLine("Terminó new " + token.str); //if (ZZ.readKey) Console.ReadKey();

                    if (la == Token.LBRACK)
                    {
                        Check(Token.LBRACK);
                        //Expr(); String finalDeExpr = token.str;
                        Expr(out item);
                        if (item.type != Tab.intType) Errors.Error("array size must be of type int");
                        Code.Load(item); //genera cod p/cargar el result de la expr
                        // new char  [10]		
                        Code.il.Emit(Code.NEWARR, type.sysType); //NEWARR de char
                        type = new Struct(Struct.Kinds.Arr, type);
                        //el nuevo type será array de char (pag 33 de T de simb)

                        Check(Token.RBRACK);
                        //if (ZZ.parser) Console.WriteLine("Pasa por [" + finalDeExpr + "]"+ "token.kind= "+Token.names[token.kind]+" token.tr="+token.str);
                    }
                    else
                    {
                        if (sym.ctor == null)
                        {
                            Console.WriteLine("Error sym.ctor == null"); if (ZZ.readKey) Console.ReadKey();
                        };
                        if (type.kind == Struct.Kinds.Class) //new Table  pag 34 de T. De Simb	  
                            Code.il.Emit(Code.NEWOBJ, sym.ctor); //emite cod p/new Table  qq1

                        else { Errors.Error("class type expected"); type = Tab.noType; }
                    }
                    item = new Item(type);
                    //item.type = type;  lo hace en el constr Item(Struct type)
                    break;
                case Token.LPAR:
                    Check(Token.LPAR);
                    //Expr();
                    Expr(out item);
                    Check(Token.RPAR);
                    break;
                default:
                    Errors.Error(ErrorStrings.INVALID_FACT);
                    item = new Item(3); //sucio: p/q no de error
                    break;
            }
    }
	/* Term = Factor { Mulop Factor } . */
    // First(Term)={ident, number, charconst, new, "("}
    // Follow(Term)={+,-}
    static void Term(out Item item)
    {
        OpCode op; Item itemSig; string opString = "";
        if (la == Token.IDENT || la == Token.NUMBER || la == Token.CHARCONST || la == Token.NEW || la == Token.LPAR)
        {
            Factor(out item); //System.Windows.Forms.MessageBox.Show("item.val.... " +item.val.ToString() );
            while ((la == Token.TIMES || la == Token.SLASH || la == Token.REM) && la != Token.EOF)
            {
                Code.cargaProgDeLaGram("OpcMulopFactor = Mulop Factor.");
                switch (la)
                {
                    case Token.TIMES:
                        Check(Token.TIMES); op = Code.MUL; opString = "mul       ";
                        Code.coloreaConRojo("token");
                        //Code.coloreaConRojo(true);
                        Code.cargaProgDeLaGram("Mulop =	'*'.");
                        if (muestraProducciones) MessageBoxCon3Preg();
                        break;
                    case Token.SLASH:
                        Check(Token.SLASH); op = Code.DIV; opString = "div       ";
                        Code.coloreaConRojo("token");
                        //Code.coloreaConRojo(true);
                        Code.cargaProgDeLaGram("Mulop =	'/'.");
                        if (muestraProducciones) MessageBoxCon3Preg();
                        break;
                    case Token.REM:
                        Check(Token.REM); op = Code.REM;
                        System.Windows.Forms.MessageBox.Show("aun no implementado");
                        break;
                    default:
                        Errors.Error(ErrorStrings.MUL_OP);
                        op = Code.REM; //sucio, tengo q ponerlo p/q no de error
                        break;
                } //Fin switch

                Code.Load(item);
                //Mulop();
                Factor(out itemSig);
                Code.Load(itemSig);
                if (item.type != Tab.intType || itemSig.type != Tab.intType)
                    Errors.Error("Debe venir un Term");

                Code.il.Emit(op);
                nroDeInstrCorriente++;
                Code.cargaInstr(opString);
                if (op == Code.MUL)
                    cil[nroDeInstrCorriente].accionInstr = AccionInstr.mul;
                else if (op == Code.DIV)
                    cil[nroDeInstrCorriente].accionInstr = AccionInstr.div;
                //System.Windows.Forms.MessageBox.Show("aun no implementado 3433rr23");
                //cil[nroDeInstrCorriente].accionInstr = AccionInstr.none;
                else
                    System.Windows.Forms.MessageBox.Show("aun no implementado 343323");
            }//Fin while
            Code.cargaProgDeLaGram("OpcMulopFactor = .");
            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false); //no hay más "*"...
            if (muestraProducciones) MessageBoxCon3Preg();
        }
        else
        {
            Errors.Error("ErrorStrings.MUL_OP");
            item = new Item(0); ///Sucio: p/q no de error
        }
    }//Fin Term
    static void Term(out Item item, System.Windows.Forms.TreeNode padre)
    {
        OpCode op; Item itemSig; string opString = "";
        if (la == Token.IDENT || la == Token.NUMBER || la == Token.CHARCONST || la == Token.NEW || la == Token.LPAR)
        {
            ///////////
            System.Windows.Forms.TreeNode hijofactor = new System.Windows.Forms.TreeNode("Factor = Designator OpcRestOfMethCall | number");
            padre.Nodes.Add(hijofactor);
            //////////
            Factor(out item,hijofactor); //System.Windows.Forms.MessageBox.Show("item.val.... " +item.val.ToString() );
            bool banderita = false;//
            while ((la == Token.TIMES || la == Token.SLASH || la == Token.REM) && la != Token.EOF)
            {
                Code.cargaProgDeLaGram("OpcMulopFactor = Mulop Factor.");

                /////////////
                System.Windows.Forms.TreeNode hijoOpcMulopFactor = new System.Windows.Forms.TreeNode("OpcMulopFactor = Mulop Factor.");
                padre.Nodes.Add(hijoOpcMulopFactor);
                System.Windows.Forms.TreeNode hijoOpcMulop = new System.Windows.Forms.TreeNode("Mulop = '*' | '/' | '%' ");
                hijoOpcMulopFactor.Nodes.Add(hijoOpcMulop);
                ////////////
                switch(la){
                    case Token.TIMES:
                        Check(Token.TIMES); op = Code.MUL;  opString = "mul       ";
                        Code.coloreaConRojo("token");
                        //Code.coloreaConRojo(true);
                        Code.cargaProgDeLaGram("Mulop =	'*'."); banderita = true;
                        //////////
                        hijoOpcMulop.Nodes.Add("Mulop =	'*'");
                        ////////
                        if (muestraProducciones) MessageBoxCon3Preg();
                        break;
                    case Token.SLASH:
                        Check(Token.SLASH); op = Code.DIV; opString = "div       "; banderita = true;
                        Code.coloreaConRojo("token");
                        //Code.coloreaConRojo(true);
                        Code.cargaProgDeLaGram("Mulop =	'/'.");
                        //////////
                        hijoOpcMulop.Nodes.Add("Mulop =	'/'");
                        ////////
                        if (muestraProducciones) MessageBoxCon3Preg();
                        break;
                    case Token.REM:////////////????????//////////
                        Check(Token.REM); op = Code.REM;
                        System.Windows.Forms.MessageBox.Show("aun no implementado");
                        break;
                    default:
                        Errors.Error(ErrorStrings.MUL_OP);
                        op = Code.REM; //sucio, tengo q ponerlo p/q no de error
                        break;
                } //Fin switch

                Code.Load(item);
                //Mulop();
                /////////////
                System.Windows.Forms.TreeNode hijoFactor = new System.Windows.Forms.TreeNode("Factor = Designator OpcRestMethCall | number");
                hijoOpcMulopFactor.Nodes.Add(hijoFactor);
                ////////////
                Factor(out itemSig,hijoFactor);
                Code.Load(itemSig);
                if (item.type != Tab.intType || itemSig.type != Tab.intType)
                    Errors.Error("Debe venir un Term");
                
                Code.il.Emit(op);
                nroDeInstrCorriente++;
                Code.cargaInstr(opString);
                if (op == Code.MUL)
                    cil[nroDeInstrCorriente].accionInstr = AccionInstr.mul;
                else if (op == Code.DIV)
                    cil[nroDeInstrCorriente].accionInstr = AccionInstr.div;
                    //System.Windows.Forms.MessageBox.Show("aun no implementado 3433rr23");
                //cil[nroDeInstrCorriente].accionInstr = AccionInstr.none;
                  else
                    System.Windows.Forms.MessageBox.Show("aun no implementado 343323");
            }//Fin while
            if (banderita == false)
            {
                padre.Nodes.Add("OpcMulopFactor = .");
                Code.cargaProgDeLaGram("OpcMulopFactor = .");
                Code.coloreaConRojo("latoken");
                //Code.coloreaConRojo(false); //no hay más "*"...
            }
            if (muestraProducciones) MessageBoxCon3Preg();
        }
        else
        {
            Errors.Error("ErrorStrings.MUL_OP");
            item = new Item(0); ///Sucio: p/q no de error
        }
    }//Fin Term
    

	/* Factor = Designator [ "(" [ ActPars ] ")" ]
	 *        | number 
	 *        | charConst
	 *        | "new" ident [ "[" Expr "]" ]
	 *        | "(" Expr ")" . */
    // First(Factor)={ident, number, charconst, new, "("}
    // Follow(Factor)={*, /, %}


    static void Factor(out Item item, System.Windows.Forms.TreeNode padre)
    {
        Struct xType;  //luego debe devolver el xType en Factor(...)
        if (la == Token.IDENT)
        {
            //////////
            System.Windows.Forms.TreeNode hijodesignator = new System.Windows.Forms.TreeNode("Designator OpcRestOfMethCall");
            padre.Nodes.Add(hijodesignator);
            /////////
            Designator(out item,hijodesignator); //en el Factor
            if (la == Token.LPAR)
            {//meth(params)
                Check(Token.LPAR);
                Code.cargaProgDeLaGram("OpcRestOfMethCall = '(' OpcActPars ')'.");
                Code.coloreaConRojo("token");
                //Code.coloreaConRojo(true); // el "("
                if (muestraProducciones) MessageBoxCon3Preg();
                if (la == Token.MINUS || la == Token.IDENT ||
                    la == Token.NUMBER || la == Token.CHARCONST ||
                    la == Token.NEW || la == Token.LPAR)
                    ActPars();  //Esta parte falta
                Check(Token.RPAR);
            }
            else
            {
                Code.cargaProgDeLaGram("OpcRestOfMethCall = .");
                Code.coloreaConRojo("latoken");
                //Code.coloreaConRojo(false); // el "("
                if (muestraProducciones) MessageBoxCon3Preg();
            }
        }
        else
            switch (la)
            {
                case Token.NUMBER:
                    Check(Token.NUMBER);
                    Code.cargaProgDeLaGram("Factor = number.");
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    /////
                    System.Windows.Forms.TreeNode hijonumber = new System.Windows.Forms.TreeNode("Factor = number.");
                    padre.Nodes.Add(hijonumber);
                    /////
                    if (muestraProducciones) MessageBoxCon3Preg();

                    item = new Item(token.val);//Nuevo
                    Code.Load(item);
                    break;
                case Token.CHARCONST:
                    Check(Token.CHARCONST);
                    item = new Item(token.val); item.type = Tab.charType; 
                    break;
                case Token.NEW: 
                    Check(Token.NEW);

                    Check(Token.IDENT);  //Deberia buscar en la Tab y verificar que sea un Tipo o una clase 
                    Symbol sym = Tab.Find(token.str);  //ident debe ser int, char, o una clase  (Table)
                    if (sym.kind != Symbol.Kinds.Type) Errors.Error("type expected");
            		Struct type = sym.type; 
                    //si es clase, sym.type contiene un puntero a los campos de esa clase

                    if (ZZ.parser) Console.WriteLine("Tab.Find(" + token.str + ") =>" + sym.ToString() + "..."); //if (ZZ.readKey) Console.ReadKey();
                    if (sym == null)
                    {
                        Errors.Error("--debe venir un tipo");//Fran
                        xType = Tab.noType;
                    }
                    else
                    {
                        xType = sym.type; //Devuelve int como tipo (Struct), no como nodo Symbol 
                        if (ZZ.parser) Console.WriteLine("Encontró " + token.str); //if (ZZ.readKey) Console.ReadKey();
                    };
                    if (ZZ.parser) Console.WriteLine("Terminó new " + token.str); //if (ZZ.readKey) Console.ReadKey();
                    
                    if (la == Token.LBRACK)
                    {
                        Check(Token.LBRACK);
                        //Expr(); String finalDeExpr = token.str;
                        Expr(out item);
                        if (item.type != Tab.intType) Errors.Error("array size must be of type int");
                        Code.Load(item); //genera cod p/cargar el result de la expr
                        // new char  [10]		
                        Code.il.Emit(Code.NEWARR, type.sysType); //NEWARR de char
                        type = new Struct(Struct.Kinds.Arr, type);
                        //el nuevo type será array de char (pag 33 de T de simb)
                        
                        Check(Token.RBRACK);
                        //if (ZZ.parser) Console.WriteLine("Pasa por [" + finalDeExpr + "]"+ "token.kind= "+Token.names[token.kind]+" token.tr="+token.str);
                    }
                    else 
                    {
                        if (sym.ctor == null)
                        {
                            Console.WriteLine("Error sym.ctor == null"); if (ZZ.readKey) Console.ReadKey();
                        };
                        if (type.kind == Struct.Kinds.Class) //new Table  pag 34 de T. De Simb	  
                            Code.il.Emit(Code.NEWOBJ, sym.ctor); //emite cod p/new Table  qq1

                        else { Errors.Error("class type expected"); type = Tab.noType; }
                    }
                    item = new Item(type); 
                       //item.type = type;  lo hace en el constr Item(Struct type)
                    break;
                case Token.LPAR:
                    Check(Token.LPAR);
                    //Expr();
                    ////////////////////////
                    padre.Nodes.Add("'('");
                                       
                    //////////////////////
                    Expr(out item,padre);
                    Check(Token.RPAR);
                    /////////////////
                    padre.Nodes.Add("')'");
                    ////////////////
                    break;
                default:
                    Errors.Error(ErrorStrings.INVALID_FACT);
                    item = new Item(3); //sucio: p/q no de error
                    break;
            }
    }

    /* Designator = ident { "." ident | "[" Expr "]" } . */
    // First(Designator)={ident}
    // Follow(Designator)={"=", "(", "++", "--", ")"}
    static void Designator(out Item item,System.Windows.Forms.TreeNode padre)
    {  ////void Main() int x,i; {    => val    = new Table;....}

        //debe buscar el designator en la tabla de simbolos
        Check(Token.IDENT); //ahora token.str="val"      y laToken= "="
        //Code.coloreaConRojo(true); ya lo mostró antes
        //if (muestraProducciones) MessageBoxCon3Preg();
        padre.Nodes.Add("Ident");
     //   Code.seleccLaProdEnLaGram(31);
    //    Code.cargaProgDeLaGram("Designator = ident  opcRestOfDesignator.");
        /////////
        System.Windows.Forms.TreeNode hijo2 = new System.Windows.Forms.TreeNode("opcRestOfDesignator = .|'.' ident | '[' Expr ']'");
        padre.Nodes.Add(hijo2);
        ////////
        Symbol sym = Tab.Find(token.str);
        if (ZZ.ParserStatem) Console.WriteLine("token.str:" + token.str);
        if (sym == null) Errors.Error(sym.name + "..no está en la Tab");

        item = new Item(sym);
        if ((la == Token.PERIOD || la == Token.LBRACK) && la != Token.EOF)
        {
            while ((la == Token.PERIOD || la == Token.LBRACK) && la != Token.EOF)//hacer do...while
            {
                //debe seguir buscando en la Tab
                if (ZZ.parser) Console.Write("field..." + token.str + " (val)"); //val
                if (la == Token.PERIOD)
                {
                    Check(Token.PERIOD); //caso del val . pos
                    Code.cargaProgDeLaGram("opcRestOfDesignator =  '.' ident.");
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                     ///////
                    System.Windows.Forms.TreeNode hijo3 = new System.Windows.Forms.TreeNode("opcRestOfDesignator =  '.' ident.");
                    hijo2.Nodes.Add(hijo3);
                    hijo3.Nodes.Add("'.'");
                    ///////
                    if (muestraProducciones) MessageBoxCon3Preg();

                    Check(Token.IDENT); //pos
                    Code.coloreaConRojo("token");
                    //Code.coloreaConRojo(true);
                    hijo3.Nodes.Add("'Ident'");
                    if (muestraProducciones) MessageBoxCon3Preg();

                    if (ZZ.parser) Console.WriteLine(" . " + token.str + " (pos)"); //pos
                    if (item.type.kind == Struct.Kinds.Class)
                    {
                        //falta Code.cargaProgDeLaGram("......
                        Code.Load(item);  //lleva el Item al  Stack

                        Symbol symField = Tab.FindSymbol(token.str, sym.type.fields);
                        //sim = Tab.FindSymbol(token.str, sym.type.fields);//pierde sim orig pero sirve,
                        // para luego hacer item = new Item(sym);

                        Struct xTypeField;
                        if (symField == null)
                        {
                            Errors.Error("..--debe venir un tipo");//Fran
                            xTypeField = Tab.noType;
                        }
                        else
                        {
                            if (ZZ.parser) Console.WriteLine("encuentra " + symField.name);
                            xTypeField = symField.type; //Devuelve int como tipo (Struct), no como nodo Symbol 
                        };
                        item.sym = symField; // Tab.FindField(token.str, item.type);
                        item.type = item.sym.type; //int        f     clase
                    }
                    else Errors.Error(sym.name + " is not an object");
                    item.kind = Item.Kinds.Field; //Field
                }
                else
                    if (la == Token.LBRACK)
                    {
                        Check(Token.LBRACK);
                        Code.cargaProgDeLaGram("opcRestOfDesignator =  '[' Expr ']'.");
                        Code.coloreaConRojo("token");
                        //Code.coloreaConRojo(true);
                        if (muestraProducciones) MessageBoxCon3Preg();

                        Code.Load(item);
                        Item itemSig;
                        Expr(out itemSig,null);

                        if (item.type.kind == Struct.Kinds.Arr)
                        {
                            if (itemSig.type != Tab.intType) Errors.Error("index must be of type int");
                            Code.Load(itemSig);  //carga el subindice en el Stack
                            itemSig.type = item.type.elemType;//Si char[10] a; => x.type quedará con char
                        }
                        else Errors.Error(sym.name + " is not an array");
                        item.kind = Item.Kinds.Elem;

                        Check(Token.RBRACK);
                    }
            }//Fin while
            //Falta Code.cargaProgDeLaGram("...
        }
        else
        {
            Code.cargaProgDeLaGram("opcRestOfDesignator =  .");
            Code.coloreaConRojo("latoken");
            //Code.coloreaConRojo(false); //lo que sigue al designator (por ej "=", en Designator = Expr)
            //////
            hijo2.Nodes.Add("opcRestOfDesignator =  .");
            //////
            if (muestraProducciones) MessageBoxCon3Preg();

            Code.Load(item);  //(item.val ya tiene valor)
            
        }
    } //Fin Designator(out Item item)
	

    /* Designator = ident { "." ident | "[" Expr "]" } . */
    // First(Designator)={ident}
    // Follow(Designator)={"=", "(", "++", "--", ")"}


	/* Relop = "==" | "!=" | ">" | ">=" | "<" | "<=" . */

    static void Relop(out int op) 
    {
        switch (la)
        {
            case Token.EQ:
                Check(Token.EQ); op = Token.EQ;
                break;
            case Token.NE:
                Check(Token.NE); op = Token.NE;
                break;
            case Token.GT:
                Check(Token.GT); op = Token.GT;
                break;
            case Token.GE:
                Check(Token.GE); op = Token.GE;
                break;
            case Token.LT:
                Check(Token.LT); op = Token.LT;
                break;
            case Token.LE:
                Check(Token.LE); op = Token.LE;
                break;
            default:
                Errors.Error(ErrorStrings.REL_OP); op = Token.EQ; //Solo para q no de error
                break;
        }
        //op = 3; // Token.EQ;  //Provisorio
    }

    static void Relop(System.Windows.Forms.TreeNode padre) //old
    {
        switch (la)
        {
            case Token.EQ:
                Check(Token.EQ);
                break;
            case Token.NE:
                Check(Token.NE);
                break;
            case Token.GT:
                Check(Token.GT);
                break;
            case Token.GE:
                Check(Token.GE);
                break;
            case Token.LT:
                Check(Token.LT);
                break;
            case Token.LE:
                Check(Token.LE);
                break;
            default:
                Errors.Error(ErrorStrings.REL_OP);
                break;
        }
        padre.Nodes.Add(la.ToString());
    }

	/* Addop = "+" | "-" . */
    static void Addop()
    {
        if (la == Token.PLUS)
            Check(Token.PLUS);
        else
            if (la == Token.MINUS)
                Check(Token.MINUS);
            else
                Errors.Error(ErrorStrings.ADD_OP);
    }

	/* Mulop = "*" | "/" | "%" . */
    static void Mulop()
    {
        if (la == Token.TIMES)
            Check(Token.TIMES);
        else
            if (la == Token.SLASH)
                Check(Token.SLASH);
            else
                if (la == Token.REM)
                    Check(Token.REM);
                else
                    Errors.Error(ErrorStrings.MUL_OP);
    }

    // Métodos agregados por Manuel para manejo de errores
    static BitArray addConjuntos(BitArray a, BitArray b)
    {
        BitArray c = (BitArray)a.Clone();
        c.Or(b);
        return c;
    }
    static void testParsing(BitArray c1, BitArray c2, int nro_error)
    {
        if (c1[la] == false) // lookahead no está en c1
        {
            Errors.Error(ErrorStrings.ADD_OP);
            c1 = addConjuntos(c1, c2);
            while (c1[la] == false)
                Scan();
        }
    }

	//////////////////////////////////////////////////////////////////////////
	
	/* Starts the analysis. 
	 * Output is written to System.out.
	 */

	public static void Parse(string prog) {
       // output = new TestTextWriter();  //Console.Out

                    //entrada del parser    salida    
        Scanner.Init(new StringReader(prog), null);  //deja en ch el 1° char de prog 
        
		
		Tab.Init();  //topScope queda apuntando al Scope p/el universe
		Errors.Init();

		curMethod = null;
		token = null;

        //Con 1° char de prog en la vble ch, ya puede comenzar el Scanner 
        laToken = new Token(1, 1);  // avoid crash when 1st symbol has scanner error
                                    //porque el Scan comienza con token = laToken

		Scan();                     // scan first symbol

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        Program();                  // start analysis  
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        
        //BitArray conjunto = new BitArray(Token.names.Length);
        //
        //conjunto[Token.EOF] = true;
        //Program(conjunto);
        Check(Token.EOF);
        //Console.WriteLine("========================================");
        //Code.il.EmitCall(Code.CALL, Code.readChar, null);

        //esto estaba de antes 
        //Code.il.Emit(OpCodes.Ldc_I4_1);
        //Code.il.Emit(OpCodes.Ldc_I4_1);
        //int[] ctorParams = { 1, 9 }; //new int[2]
        //Type[] ctorParams = new Type[] { typeof(int), typeof(int) };

        //Code.il.EmitCall(Code.CALL, Code.writeInt, ctorParams);
        
        //Console.WriteLine("========================================");
        //if (ZZ.readKey) Console.ReadKey();
    }
	
	/* Handles all compiler errors. */
	public class Errors {
		/* Minimal number of tokens between errors.
		 * Errors are only reported if error distance is greater than this. */
		const int minDist = 3;

		/* Current distance from last syntax error. */
		public static int dist;
		
		/* Error count. */
		static int cnt;
		
		/* Print error message to output and count reported errors. */
		public static void Error (string msg) {
			/*---------------------------------*/
			/*----- insert your code here -----*/
			/*---------------------------------*/
            cnt++;
            //Console.WriteLine("Error: line {0}, col {1}: {2}", token.line, token.col, msg);
            int linea = token.line; int col = token.col; 
            int sizeToken;
            if (token.str == null) sizeToken = 1; 
                      else sizeToken = token.str.Length;   //aqui
            if (sizeToken < 1) sizeToken = 1;
            throw new ErrorMio(linea, col, sizeToken, msg);
            //richTextBox1.Select(richTextBox1.GetFirstCharIndexFromLine(linea) + col,
            //                    sizeToken);
            //richTextBox1.SelectionColor = Color.Red;
            //Console.WriteLine(" toque una tecla para continuar");
            //if (ZZ.readKey) Console.ReadKey();
		}//Fin class Error
		
		public static int Count { get { return cnt; } }
		
		public static void Init () { cnt = 0; Reset(); }
		
		public static void Reset () 
        {
			dist = minDist;  // don't skip errors any more
		}
	}
}//Fin class Parser

}//Fin namespace

