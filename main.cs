using System;
using System.IO;

namespace Bakera.RedFaceLint{

	public class App{
		public static int Main(){

			char c = '\x9AB6';
			Console.WriteLine(c);
			Console.WriteLine((int)c);

			string f = "data/test.html";
			FileInfo file = new FileInfo(f);
			Html5Parser p = new Html5Parser();
			p.Load(f);
//			p.Parse();

			

			return 0;
		}


	}

	public enum ParserMode{
		Initial,
		BeforeHtml,
		BeforeHead,
		InHead,
		InHeadNoscript,
		AfterHead,
		InBody,
		Text,
		InTable,
		InTableText,
		InCaption,
		InColumnGroup,
		InTableBody,
		InRow,
		InCell,
		InSelect,
		InSelectInTable,
		AfterBody,
		InFrameset,
		AfterFrameset,
		AfterAfterBody,
		AfterAfterFrameset,
	}


	public enum TokenState{
		DOCTYPE,
		StartTag,
		EndTag,
		Comment,
		Character,
		EndOfFile
	}

}



