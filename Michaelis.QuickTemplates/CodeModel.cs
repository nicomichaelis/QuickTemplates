
#nullable enable

using System.Collections.Generic;

namespace Michaelis.QuickTemplates;

abstract record ModelNode();

record FileNode(string Filename, string Namespace, List<ModelNode> Head, List<ModelNode> Content, List<ModelNode> Bottom) : ModelNode;

record ClassNode(string Classname, List<ModelNode> Head, List<ModelNode> Content, List<ModelNode> Bottom) : ModelNode;

record SimplePropertyNode(string Propertyname, string PropertyType) : ModelNode;
