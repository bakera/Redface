using System;
using System.IO;
using System.Xml;

namespace Bakera.RedFace{

	public class App{
		public static int Main(){

			RedFaceParser p = new RedFaceParser();

			try{

				InitialInsertionMode temp = new InitialInsertionMode();

				string f = "data/test.html";
				FileInfo file = new FileInfo(f);

				p.ParserEventRaised += WriteEvent;

				using(FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)){
					p.Parse(fs);
				}
				var logs = p.GetLogs();
				foreach(ParserLog log in logs){
					Console.WriteLine("{0}行{1}文字: {2}", log.Line.Number, log.ColumnNumber, log.Message);
					Console.WriteLine(" {0}", log.Line.Data);
				}

				Console.WriteLine("パース開始: {0}", p.StartTime);
				Console.WriteLine("パース終了: {0}", p.EndTime);
				Console.WriteLine("パース時間: {0}", p.EndTime - p.StartTime);
				Console.WriteLine();
				Console.WriteLine("========");
//				Console.WriteLine(p.Document.OuterXml);

				return 0;
			} catch(Exception e){
				Console.WriteLine(e);
				return 1;
			}
		}

		public static void WriteEvent(Object sender, ParserEventArgs e){
			if(e.Message != null) Console.WriteLine(e.Message);
//			if(e.Token != null && !(e.Token is CharacterToken)) Console.WriteLine(e.Token);
		}

	}

	public enum InsetionMode{
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



