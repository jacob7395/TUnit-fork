"use strict";(self.webpackChunktunit_docs_site=self.webpackChunktunit_docs_site||[]).push([[1500],{7091:(t,e,n)=>{n.r(e),n.d(e,{assets:()=>c,contentTitle:()=>r,default:()=>l,frontMatter:()=>o,metadata:()=>s,toc:()=>d});const s=JSON.parse('{"id":"tutorial-basics/method-data-source","title":"Method Data Sources","description":"A limitation of passing data in with [Arguments(...)] is that the data must be constant values. For example, we can\'t new up an object and pass it into this attribute as an argument. This is a constraint of the language and we can\'t change that.","source":"@site/docs/tutorial-basics/method-data-source.md","sourceDirName":"tutorial-basics","slug":"/tutorial-basics/method-data-source","permalink":"/TUnit/docs/tutorial-basics/method-data-source","draft":false,"unlisted":false,"tags":[],"version":"current","sidebarPosition":5,"frontMatter":{"sidebar_position":5},"sidebar":"tutorialSidebar","previous":{"title":"Injectable Class Data Source","permalink":"/TUnit/docs/tutorial-basics/class-data-source"},"next":{"title":"Matrix Tests","permalink":"/TUnit/docs/tutorial-basics/matrix-tests"}}');var a=n(4848),i=n(8453);const o={sidebar_position:5},r="Method Data Sources",c={},d=[];function u(t){const e={code:"code",h1:"h1",header:"header",li:"li",p:"p",pre:"pre",ul:"ul",...(0,i.R)(),...t.components};return(0,a.jsxs)(a.Fragment,{children:[(0,a.jsx)(e.header,{children:(0,a.jsx)(e.h1,{id:"method-data-sources",children:"Method Data Sources"})}),"\n",(0,a.jsxs)(e.p,{children:["A limitation of passing data in with ",(0,a.jsx)(e.code,{children:"[Arguments(...)]"})," is that the data must be ",(0,a.jsx)(e.code,{children:"constant"})," values. For example, we can't new up an object and pass it into this attribute as an argument. This is a constraint of the language and we can't change that."]}),"\n",(0,a.jsx)(e.p,{children:"If we want test data represented in the form of objects, or just to use something that isn't a constant, we can declare a test data source."}),"\n",(0,a.jsxs)(e.p,{children:[(0,a.jsx)(e.code,{children:"MethodDataSource"})," has two options:"]}),"\n",(0,a.jsxs)(e.ul,{children:["\n",(0,a.jsx)(e.li,{children:"If you pass in one argument, this is the method name containing your data. TUnit will assume this is in the current test class."}),"\n",(0,a.jsxs)(e.li,{children:["If you pass in two arguments, the first should be the ",(0,a.jsx)(e.code,{children:"Type"})," of the class containing your test source data method, and the second should be the name of the method."]}),"\n"]}),"\n",(0,a.jsxs)(e.p,{children:["If methods are returning reference types, they should return a ",(0,a.jsx)(e.code,{children:"Func<T>"})," rather than just a ",(0,a.jsx)(e.code,{children:"T"})," - This ensures each test has its own instance of that object and tests aren't sharing objects which could lead to unintended side effects."]}),"\n",(0,a.jsx)(e.p,{children:"Here's an example returning a simple object:"}),"\n",(0,a.jsx)(e.pre,{children:(0,a.jsx)(e.code,{className:"language-csharp",children:"using TUnit.Assertions;\nusing TUnit.Assertions.Extensions;\nusing TUnit.Core;\n\nnamespace MyTestProject;\n\npublic record AdditionTestData(int Value1, int Value2, int ExpectedResult);\n\npublic static class MyTestDataSources\n{\n    public static Func<AdditionTestData> AdditionTestData()\n    {\n        return () => new AdditionTestData(1, 2, 3);\n    }\n}\n\npublic class MyTestClass\n{\n    [Test]\n    [MethodDataSource(typeof(MyTestDataSources), nameof(MyTestDataSources.AdditionTestData))]\n    public async Task MyTest(AdditionTestData additionTestData)\n    {\n        var result = Add(additionTestData.Value1, additionTestData.Value2);\n\n        await Assert.That(result).IsEqualTo(additionTestData.ExpectedResult);\n    }\n\n    private int Add(int x, int y)\n    {\n        return x + y;\n    }\n}\n"})}),"\n",(0,a.jsx)(e.p,{children:"This can also accept tuples if you don't want to create lots of new types within your test assembly:"}),"\n",(0,a.jsx)(e.pre,{children:(0,a.jsx)(e.code,{className:"language-csharp",children:"using TUnit.Assertions;\nusing TUnit.Assertions.Extensions;\nusing TUnit.Core;\n\nnamespace MyTestProject;\n\npublic static class MyTestDataSources\n{\n    public static Func<(int, int, int)> AdditionTestData()\n    {\n        return () => (1, 2, 3);\n    }\n}\n\npublic class MyTestClass\n{\n    [Test]\n    [MethodDataSource(typeof(MyTestDataSources), nameof(MyTestDataSources.AdditionTestData))]\n    public async Task MyTest(int value1, int value2, int expectedResult)\n    {\n        var result = Add(value1, value2);\n\n        await Assert.That(result).IsEqualTo(expectedResult);\n    }\n\n    private int Add(int x, int y)\n    {\n        return x + y;\n    }\n}\n"})}),"\n",(0,a.jsxs)(e.p,{children:["This attribute can also accept ",(0,a.jsx)(e.code,{children:"IEnumerable<>"}),". For each item returned, a new test will be created with that item passed in to the parameters."]}),"\n",(0,a.jsx)(e.p,{children:"Here's an example where the test would be invoked 3 times:"}),"\n",(0,a.jsx)(e.pre,{children:(0,a.jsx)(e.code,{className:"language-csharp",children:"using TUnit.Assertions;\nusing TUnit.Assertions.Extensions;\nusing TUnit.Core;\n\nnamespace MyTestProject;\n\npublic record AdditionTestData(int Value1, int Value2, int ExpectedResult);\n\npublic static class MyTestDataSources\n{\n    public static IEnumerable<Func<AdditionTestData>> AdditionTestData()\n    {\n        yield return () => new AdditionTestData(1, 2, 3);\n        yield return () => new AdditionTestData(2, 2, 4);\n        yield return () => new AdditionTestData(5, 5, 10);\n    }\n}\n\npublic class MyTestClass\n{\n    [Test]\n    [MethodDataSource(typeof(MyTestDataSources), nameof(MyTestDataSources.AdditionTestData))]\n    public async Task MyTest(AdditionTestData additionTestData)\n    {\n        var result = Add(additionTestData.Value1, additionTestData.Value2);\n\n        await Assert.That(result).IsEqualTo(additionTestData.ExpectedResult);\n    }\n\n    private int Add(int x, int y)\n    {\n        return x + y;\n    }\n}\n"})}),"\n",(0,a.jsx)(e.p,{children:"This can also accept tuples if you don't want to create lots of new types within your test assembly:"}),"\n",(0,a.jsx)(e.pre,{children:(0,a.jsx)(e.code,{className:"language-csharp",children:"using TUnit.Assertions;\nusing TUnit.Assertions.Extensions;\nusing TUnit.Core;\n\nnamespace MyTestProject;\n\npublic static class MyTestDataSources\n{\n    public static IEnumerable<Func<(int, int, int)>> AdditionTestData()\n    {\n        yield return () => (1, 2, 3);\n        yield return () => (2, 2, 4);\n        yield return () => (5, 5, 10);\n    }\n}\n\npublic class MyTestClass\n{\n    [Test]\n    [MethodDataSource(typeof(MyTestDataSources), nameof(MyTestDataSources.AdditionTestData))]\n    public async Task MyTest(int value1, int value2, int expectedResult)\n    {\n        var result = Add(value1, value2);\n\n        await Assert.That(result).IsEqualTo(expectedResult);\n    }\n\n    private int Add(int x, int y)\n    {\n        return x + y;\n    }\n}\n"})})]})}function l(t={}){const{wrapper:e}={...(0,i.R)(),...t.components};return e?(0,a.jsx)(e,{...t,children:(0,a.jsx)(u,{...t})}):u(t)}},8453:(t,e,n)=>{n.d(e,{R:()=>o,x:()=>r});var s=n(6540);const a={},i=s.createContext(a);function o(t){const e=s.useContext(i);return s.useMemo((function(){return"function"==typeof t?t(e):{...e,...t}}),[e,t])}function r(t){let e;return e=t.disableParentContext?"function"==typeof t.components?t.components(a):t.components||a:o(t.components),s.createElement(i.Provider,{value:e},t.children)}}}]);