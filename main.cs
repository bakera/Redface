using System;
using System.IO;

namespace Bakera.RedFaceLint{

	public class App{
		public static int Main(){

			try{
				string f = "data/test.html";
				FileInfo file = new FileInfo(f);
				RedFaceParser p = new RedFaceParser();
				p.Load(f);
				p.Parse();

				var logs = p.GetLogs();
				foreach(ParserLog log in logs){
					Console.WriteLine("{0}行{1}文字: {2}", log.Line.Number, log.ColumnNumber, log.Message);
					Console.WriteLine(" {0}", log.Line.Data);
				}

				return 0;
			} catch(Exception e){
				Console.WriteLine(e);
				return 1;
			}
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



}



