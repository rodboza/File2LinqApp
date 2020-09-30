using System;  
using System.Collections.Generic;
using System.IO;
using System.Linq;  
using System.Reflection;  
using System.Reflection.Emit;  
using System.Text;  
using System.Threading.Tasks;
using File2LinqApp.Domain;

namespace File2LinqApp.Core
{

    public class File2Linq  
    {  
        AssemblyName asemblyName;  
        Type type;
        public string[] columnNames;
        Type[] types;
        public LinhaBase[] rows;

        public File2Linq( string ClassName, string nomeArquivo )
        {
            this.asemblyName = new AssemblyName(ClassName);

            string[] allLines = File.ReadAllLines(nomeArquivo);
            this.columnNames = allLines.FirstOrDefault().Split(";");
            this.types = columnNames.Select( header => typeof(string)).ToArray<Type>();
            this.type = CreateType(columnNames, types);
            this.rows = allLines.Skip(1).Select( linha =>  NewObject (linha.Split(";"))).ToArray<LinhaBase>();

        }

        public File2Linq(Arquivo arquivoPrincipal): this( arquivoPrincipal.Nome, arquivoPrincipal.Caminho)
        {
        }

        private Type CreateType(string[] propertyNames, Type[] types)
        {
            if (propertyNames.Length != types.Length)
            {
                Console.WriteLine("The number of property names should match their corresopnding types number");
            }

            TypeBuilder DynamicClass = this.CreateClass();
            this.CreateConstructor(DynamicClass);
            
            for (int ind = 0; ind < propertyNames.Count(); ind++)
            {
                CreateProperty(DynamicClass, propertyNames[ind], types[ind]);
            }

            return DynamicClass.CreateType();
        }

        public LinhaBase NewObject (string[] values) 
        {
            LinhaBase newobject = (LinhaBase) Activator.CreateInstance(this.type);

            for (int i = 0; i < this.columnNames.Count(); i++)
            {
                //Type tipo = obj.GetType();
                newobject.GravaValor(this.columnNames[i],values[i]);

            }
            return newobject;
        }



        private TypeBuilder CreateClass()  
        {  
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(this.asemblyName, AssemblyBuilderAccess.Run);  
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");  
            TypeBuilder typeBuilder = moduleBuilder.DefineType(this.asemblyName.FullName  
            , TypeAttributes.Public |  
            TypeAttributes.Class |  
            TypeAttributes.AutoClass |  
            TypeAttributes.AnsiClass |  
            TypeAttributes.BeforeFieldInit |  
            TypeAttributes.AutoLayout  
            , typeof(LinhaBase) );  
            return typeBuilder;  
        }  
        private void CreateConstructor( TypeBuilder typeBuilder )  
        {  
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName); 

        }  
        private void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)  
        {  
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);  

            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);  
            MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);  
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();  

            getIl.Emit(OpCodes.Ldarg_0);  
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);  
            getIl.Emit(OpCodes.Ret);  

            MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,  
                                            MethodAttributes.Public |  
                                            MethodAttributes.SpecialName |  
                                            MethodAttributes.HideBySig,  
                                            null, new[] { propertyType });  

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();  
            Label modifyProperty = setIl.DefineLabel();  
            Label exitSet = setIl.DefineLabel();  

            setIl.MarkLabel(modifyProperty);  
            setIl.Emit(OpCodes.Ldarg_0);  
            setIl.Emit(OpCodes.Ldarg_1);  
            setIl.Emit(OpCodes.Stfld, fieldBuilder);  

            setIl.Emit(OpCodes.Nop);  
            setIl.MarkLabel(exitSet);  
            setIl.Emit(OpCodes.Ret);  

            propertyBuilder.SetGetMethod(getPropMthdBldr);  
            propertyBuilder.SetSetMethod(setPropMthdBldr);  
        }  
    }
}