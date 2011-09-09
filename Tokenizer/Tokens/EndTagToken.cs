using System;

namespace Bakera.RedFace{

/*
 Start and end tag tokens have a tag name, a self-closing flag, and a list of attributes, each of which has a name and a value.
 When a start or end tag token is created, its self-closing flag must be unset (its other state is that it be set), and its attributes list must be empty.
 Comment and character tokens have data.
*/

	public class EndTagToken : TagToken{}
}
