#nullable enable

using System.Collections.Generic;

namespace Michaelis.QuickTemplates;

abstract record ModelNode();

record FileNode(string Filename, string Namespace, IList<ModelNode> Head, IList<ModelNode> Content, IList<ModelNode> Bottom) : ModelNode;

record ClassNode(string Classname, string Modifier, IList<ModelNode> Head, IList<ModelNode> Content, IList<ModelNode> Bottom) : ModelNode;

record UsingNode(string Namespace) : ModelNode;

record SimplePropertyNode(string Propertyname, string PropertyType) : ModelNode;

record AttributeNode(string Value) : ModelNode;

record FixedLineNode(string Content, bool Indented) : ModelNode;

record LineInfoNode(string Filename, int Line) : ModelNode;

record LineEndInfoNode(FinishLineInfoMode Mode) : ModelNode;
