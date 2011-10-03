using System;
using System.IO;
using System.Xml;

namespace Bakera.RedFace{

	public class App{
		public static int Main(){

			RedFaceParser p = new RedFaceParser();

			try{

				
				string f = "data/test.html";
				FileInfo file = new FileInfo(f);
//				p.TokenStateChanged += WriteTokenState;
//				p.InsertionModeChanged += WriteInsertionMode;
				p.DocumentModeChanged += WriteDocumentMode;
				p.ParseErrorRaised += WriteError;
//				p.TokenCreated += WriteToken;

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
//				Console.WriteLine("========");
//				Console.WriteLine(p.Document.OuterXml);

				return 0;
			} catch(Exception e){
				Console.WriteLine(e);
				return 1;
			}
		}

		public static void WriteTokenState(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			Console.WriteLine(p.CurrentTokenState);
		}

		public static void WriteInsertionMode(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			Console.WriteLine("{0} ({1})", p.CurrentInsertionMode, p.StackOfOpenElements);
		}

		public static void WriteDocumentMode(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			Console.WriteLine("DocumentMode: {0}", p.Document.DocumentMode);
		}

		public static void WriteError(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			ParseErrorEventArgs pe = e as ParseErrorEventArgs;
			Console.WriteLine(pe.Message);
			Console.WriteLine( p.StackOfOpenElements);
		}

		public static void WriteToken(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			ParserTokenEventArgs pte = e as ParserTokenEventArgs;
			Token t = pte.Token;
			Console.WriteLine("Token: {0}", t);
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



