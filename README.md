# Quick Templates

This repository contains a .NET tool for generating configurable runtime text
templates for C#.

## Description

The quick template tool generates runtime text templates (RTT) for C#. RTTs
produce text strings in your C# programs.

A template is a mixture of text- and code-fragments. The text-fragments
appear in the output text of the RTT unchanged. The code-fragments control the
output of text-fragments via control structures (e.g. 'if' and 'else') or
they provide values and other variable content of the output string.

The quick template tool is in many ways similar to the RTTs produced by T4. Its
main enhancements are:

* indentatation.
* transform method parameters.
* recursive transform.
* possibility of overriding format providers.

## Documentation

Currently there exists no user documentation of the quick template tool. Since
the quick template tool uses itself to generate its own templates you may want
to inspect these [templates](./Michaelis.QuickTemplates/CsTemplates/) for
reference.
