﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace My.Labs.Translator {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("My.Labs.Translator.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to grammar : syntax ;					 
        ///syntax : rule rules-more ;			 
        ///rules-more : syntax 				 
        ///		   | &apos;&apos;;					 
        ///rule : rule-name &apos;:&apos; alternation-expression &apos;;&apos;	;
        ///alternation-expression : expression expression-more;
        ///expression-more : &apos;|&apos; alternation-expression
        ///			    | &apos;&apos;;
        ///rule-name : non-terminal;			 
        ///expression : token tokens-more ;	 
        ///tokens-more : expression		 
        ///		    | &apos;&apos; ;				 
        ///token : terminal			 
        ///	  | non-terminal ;.
        /// </summary>
        internal static string BNFGrammar {
            get {
                return ResourceManager.GetString("BNFGrammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;signal-program&gt; ::= &lt;program&gt;
        ///&lt;program&gt; ::= PROCEDURE &lt;procedure-identifier&gt; &lt;parameters-list&gt; ; &lt;declarations&gt; &lt;block&gt; ;
        ///&lt;block&gt; ::= BEGIN &lt;statements-list&gt; END
        ///&lt;statements-list&gt; ::= &lt;empty&gt;
        ///&lt;parameters-list&gt; ::= ( &lt;declarations-list&gt; ) | &lt;empty&gt;
        ///&lt;declarations-list&gt; ::= &lt;declaration&gt; &lt;declarations-list&gt; | &lt;empty&gt;
        ///&lt;declaration&gt; ::= &lt;variable-identifier&gt; : &lt;attribute&gt; ;
        ///&lt;attribute&gt; ::= INTEGER | FLOAT
        ///&lt;declarations&gt; ::= &lt;constant-declarations&gt;
        ///&lt;constant-declarations&gt; ::= CONST &lt;constant-declaration [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Grammar {
            get {
                return ResourceManager.GetString("Grammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;signal-program&gt; ::= &lt;program&gt;
        ///&lt;program&gt; ::= PROCEDURE &lt;procedure-identifier&gt; &lt;parameters-list&gt; ; &lt;declarations&gt; &lt;block&gt; ;
        ///&lt;block&gt; ::= BEGIN &lt;statements-list&gt; END
        ///&lt;statements-list&gt; ::= &lt;empty&gt;
        ///&lt;parameters-list&gt; ::= ( &lt;declarations-list&gt; ) | &lt;empty&gt;
        ///&lt;declarations-list&gt; ::= &lt;declaration&gt; &lt;declarations-list&gt; | &lt;empty&gt;
        ///&lt;declaration&gt; ::= &lt;variable-identifier&gt; : &lt;attribute&gt; ;
        ///&lt;attribute&gt; ::= INTEGER | FLOAT
        ///&lt;declarations&gt; ::= &lt;constant-declarations&gt;
        ///&lt;constant-declarations&gt; ::= CONST &lt;constant-declaration [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Grammar2 {
            get {
                return ResourceManager.GetString("Grammar2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to whitespace	: &quot; \n\r\v\t&quot;, 12 ;
        ///letter		: &quot;ABCDEFGHIGKLMNOPQRSTUVWXYZ&quot;, LC ;
        ///digit		: &quot;0123456789&quot; ;
        ///delimiter	: &quot;():;-&quot; ;
        ///
        ///
        ///.
        /// </summary>
        internal static string Lab_LexerGrammar {
            get {
                return ResourceManager.GetString("Lab_LexerGrammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to signal-program				: program ;
        ///program						: &apos;PROCEDURE&apos; procedure-identifier parameters-list &apos;;&apos; declarations block &apos;;&apos; ;
        ///block						: &apos;BEGIN&apos; statements-list &apos;END&apos; ;
        ///statements-list				: &apos;&apos; ;
        ///parameters-list				: &apos;(&apos; declarations-list &apos;)&apos; 
        ///							| &apos;&apos; ;
        ///declarations-list			: declaration declarations-list 
        ///							| &apos;&apos; ;
        ///declaration					: variable-identifier &apos;:&apos; attribute ;
        ///attribute					: &apos;INTEGER&apos; 
        ///							| &apos;FLOAT&apos; ;
        ///declarations				: constant-declarations ;
        ///constant-declarations		: &apos;CONST&apos; co [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Lab_SyntacticGrammar {
            get {
                return ResourceManager.GetString("Lab_SyntacticGrammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to lex		: types ;
        ///types	: type types
        ///		| &apos;&apos;;
        ///type	: type-name &apos;:&apos; elements &apos;;&apos; ;
        ///type-name	: non-terminal ;
        ///elements	: element elements-more ;
        ///elements-more	: &apos;,&apos; element
        ///				| &apos;&apos; ;
        ///element : terminal ;.
        /// </summary>
        internal static string MetaLexGrammar {
            get {
                return ResourceManager.GetString("MetaLexGrammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;semantic_definition&gt; ::= { &lt;object_set&gt; }
        ///&lt;object_set&gt; ::= &lt;object&gt; &lt;object_set&gt;
        ///&lt;object_set&gt; ::= &lt;single_object&gt;
        ///&lt;single_object&gt; ::= &lt;object&gt;
        ///&lt;object&gt; ::= &lt;simple_object&gt;
        ///&lt;object&gt; ::= &lt;reference&gt;
        ///&lt;object&gt; ::= &lt;delimiter&gt;
        ///&lt;simple_object&gt; ::= &lt;string&gt;
        ///&lt;reference&gt; ::= [ &lt;clause&gt; ]
        ///&lt;clause&gt; ::= &lt;integer&gt;
        ///&lt;integer&gt; ::= &lt;nonterminal_number&gt;
        ///&lt;delimiter&gt; ::= _
        ///&lt;delimiter&gt; ::= @
        ///.
        /// </summary>
        internal static string MetaSemanticGrammar {
            get {
                return ResourceManager.GetString("MetaSemanticGrammar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;signal-program&gt; ::= &lt;program&gt;
        ///&lt;program&gt; ::= PROCEDURE &lt;procedure-identifier&gt; &lt;parameters-list&gt; ; &lt;declarations&gt; &lt;block&gt; ; {@[4][3][2]Buf:}
        ///
        ///&lt;parameters-list&gt; ::= ( &lt;declarations-list&gt; ) | &lt;empty&gt;										 {[1]}
        ///&lt;declarations-list&gt; ::= &lt;declaration&gt; &lt;declarations-list&gt; | &lt;empty&gt;                          {[2][1]}
        ///&lt;declaration&gt; ::= &lt;variable-identifier&gt; : &lt;attribute&gt; ;										 {[2][1]@Iden_Buf_POP}
        ///&lt;attribute&gt; ::= INTEGER																		 {Buf:=&apos;dd&apos;}
        ///&lt;attribute&gt; ::= FLOAT																		 {Buf:=&apos;d [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Sema {
            get {
                return ResourceManager.GetString("Sema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Axiom&gt; ::= &lt;S&gt;
        ///&lt;S&gt; ::= &lt;F&gt;
        ///&lt;S&gt; ::= ( &lt;S&gt; + &lt;F&gt; )
        ///&lt;F&gt; ::= a.
        /// </summary>
        internal static string TestGrammar1 {
            get {
                return ResourceManager.GetString("TestGrammar1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to signal-program				: program ;
        ///program						: &apos;PROCEDURE&apos; procedure-identifier parameters-list &apos;;&apos; declarations block &apos;;&apos; ;
        ///block						: &apos;BEGIN&apos; statements-list &apos;END&apos; ;
        ///statements-list				: &apos;&apos; ;
        ///parameters-list				: &apos;(&apos; declarations-list &apos;)&apos; 
        ///							| &apos;&apos; ;
        ///declarations-list			: declaration declarations-list 
        ///							| &apos;&apos; ;
        ///declaration					: variable-identifier &apos;:&apos; attribute ;
        ///attribute					: &apos;INTEGER&apos; 
        ///							| &apos;FLOAT&apos; ;
        ///declarations				: constant-declarations ;
        ///constant-declarations		: &apos;CONST&apos; co [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TestGrammar2 {
            get {
                return ResourceManager.GetString("TestGrammar2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PROCEDURE PROC1 ( X : INTEGER; Y : FLOAT; ) ;
        ///CONST 
        ///	PI100 = 314;
        ///	MINUS = -1;
        ///	N = 5;
        ///BEGIN
        ///	
        ///END;.
        /// </summary>
        internal static string TestProgram1 {
            get {
                return ResourceManager.GetString("TestProgram1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PROCEDURE proc2();
        ///BEGIN
        ///	(* Nothing here *)
        ///END;.
        /// </summary>
        internal static string TestProgram2 {
            get {
                return ResourceManager.GetString("TestProgram2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PROCEDURE proc3 ;
        ///BEGIN
        ///	
        ///END.
        /// </summary>
        internal static string TestProgram3 {
            get {
                return ResourceManager.GetString("TestProgram3", resourceCulture);
            }
        }
    }
}