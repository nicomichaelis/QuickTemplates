#nullable enable

using System.Collections.Generic;

namespace Michaelis.QuickTemplates;

abstract record ModelNode();

record FileNode(string Filename, string Namespace, IList<ModelNode> Head, IList<ModelNode> Content, IList<ModelNode> Bottom) : ModelNode;

record ClassNode(string Classname, string Modifier, string InheritsFrom, IList<ModelNode> Head, IList<ModelNode> Content, IList<ModelNode> Bottom) : ModelNode;

record UsingNode(string Namespace) : ModelNode;

record SimplePropertyNode(string Propertyname, string PropertyType, string Modifier, string GetAccessor, string SetAccessor, string Initializer) : ModelNode;

record AttributeNode(string Value) : ModelNode;

record FixedLineNode(string Content, bool Indented) : ModelNode;

record LineInfoNode(string Filename, int Line) : ModelNode;

record LineEndInfoNode(FinishLineInfoMode Mode) : ModelNode;

record ContextClassCodeNode() : ModelNode;

record BaseClassCodeNode(string Classname) : ModelNode;

record MethodNode(string Modifier, string ReturnType, string MethodName, IList<ModelNode> Parameters, IList<ModelNode> Head, IList<ModelNode> Content) : ModelNode;

record ParameterNode(string ParameterType, string ParameterName) : ModelNode;