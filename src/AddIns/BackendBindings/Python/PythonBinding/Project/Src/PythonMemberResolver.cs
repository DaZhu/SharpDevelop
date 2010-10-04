﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using ICSharpCode.SharpDevelop.Dom;

namespace ICSharpCode.PythonBinding
{
	/// <summary>
	/// Resolves properties, events and fields.
	/// </summary>
	public class PythonMemberResolver : IPythonResolver
	{
		PythonClassResolver classResolver;
		PythonLocalVariableResolver localVariableResolver;
		PythonResolverContext resolverContext;
		
		public PythonMemberResolver(PythonClassResolver classResolver, PythonLocalVariableResolver localVariableResolver)
		{
			this.classResolver = classResolver;
			this.localVariableResolver = localVariableResolver;
		}
		
		public ResolveResult Resolve(PythonResolverContext resolverContext)
		{
			this.resolverContext = resolverContext;
			IMember member = FindMember();
			return CreateResolveResult(member);
		}
		
		IMember FindMember()
		{
			return FindMember(resolverContext.Expression);
		}
		
		IMember FindMember(string expression)
		{
			MemberName memberName = new MemberName(expression);
			if (memberName.HasName) {
				IClass c = FindClass(memberName.Type);
				if (c != null) {
					return FindMemberInClass(c, memberName.Name);
				} else {
					return FindMemberInParent(memberName);
				}
			}
			return null;
		}
		
		IClass FindClass(string className)
		{
			IClass c = FindClassFromClassResolver(className);
			if (c != null) {
				return c;
			}
			return FindClassFromLocalVariableResolver(className);
		}
		
		IClass FindClassFromClassResolver(string className)
		{
			return classResolver.GetClass(resolverContext, className);
		}
		
		IClass FindClassFromLocalVariableResolver(string localVariableName)
		{
			 MemberName memberName = new MemberName(localVariableName);
			 if (!memberName.HasName) {
			 	string typeName = localVariableResolver.Resolve(localVariableName, resolverContext.FileContent);
		 		return FindClassFromClassResolver(typeName);
			 }
			 return null;
		}
		
		ResolveResult CreateResolveResult(IMember member)
		{
			if (member != null) {
				if (member is IMethod) {
					return new PythonMethodGroupResolveResult(member.DeclaringType, member.Name);
				}
				return new MemberResolveResult(null, null, member);
			}
			return null;
		}
		
		IMember FindMemberInClass(IClass matchingClass, string memberName)
		{
			PythonClassMembers classMembers = new PythonClassMembers(matchingClass);
			return classMembers.FindMember(memberName);
		}
		
		IMember FindMemberInParent(MemberName memberName)
		{
			IMember parentMember = FindMember(memberName.Type);
			if (parentMember != null) {
				return FindMemberInParent(parentMember, memberName.Name);
			}
			return null;
		}
		
		IMember FindMemberInParent(IMember parentMember, string memberName)
		{
			IClass parentMemberClass = parentMember.ReturnType.GetUnderlyingClass();
			return FindMemberInClass(parentMemberClass, memberName);
		}
	}
}
