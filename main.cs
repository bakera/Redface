using System;
using System.IO;

namespace Bakera.RedFace{

	public class App{
		public static int Main(){

			try{
				string f = "data/test.html";
				FileInfo file = new FileInfo(f);
				RedFaceParser p = new RedFaceParser();
				p.TokenStateChanged += Write;
				p.ParseErrorRaised += WriteError;
				p.CharacterReferenced += WriteCharRef;

				p.Load(file);
				p.Parse();
				var logs = p.GetLogs();
				foreach(ParserLog log in logs){
					Console.WriteLine("{0}行{1}文字: {2}", log.Line.Number, log.ColumnNumber, log.Message);
					Console.WriteLine(" {0}", log.Line.Data);
				}

				Console.WriteLine("パース開始: {0}", p.StartTime);
				Console.WriteLine("パース終了: {0}", p.EndTime);
				Console.WriteLine("処理バイト数: {0}", p.CurrentPosition);
				Console.WriteLine(p.EmittedToken);

				return 0;
			} catch(Exception e){
				Console.WriteLine(e);
				return 1;
			}
		}

		public static void Write(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			Console.WriteLine(p.CurrentTokenState.Name);
		}

		public static void WriteError(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			ParseErrorEventArgs pe = e as ParseErrorEventArgs;
			Console.WriteLine(pe.Message);
		}

		public static void WriteCharRef(Object sender, EventArgs e){
			RedFaceParser p = sender as RedFaceParser;
			CharacterReferencedEventArgs cre = e as CharacterReferencedEventArgs;
			Console.WriteLine("charref: {0}→{1}", cre.OriginalString, cre.Result);
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



