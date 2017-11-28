using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CL.Herramientas
{
    public static class FuncionesAuxiliares
    {
        /*Declaramos la clase y los metodos como static para acceder sin necesidad de instanciar*/

        //Verificacion de Usuario
        public static bool EsAdmin(UsuarioDTO miUsuario)
        {
            /*Retornara true si es admin y false si no lo es*/
            return miUsuario.EsAdmin == true;
        }

        //Verificamos que una fecha es de hace 18 años o mas TIENE ERROR DE 1 DIA
        public static bool MayorDeEdad(DateTime fechaNacimiento)
        {
            return ((DateTime.Today.AddTicks(-fechaNacimiento.Ticks).Year - 1) >= 18);
        }

        //Verifica se la contraseña que nos pasan cumple con las condiciones de seguridad
        public static bool ContrasenaEsValida(string unaContraseña)
        {
            //Si viene vacia no pasa nada porque No va a encontrar ni numeros, ni mayusculas, ni min, y retorna false: 
            //De todas formas controlamos que tenga entre 8 y 16 caracteres
            if(unaContraseña.Length < 8 || unaContraseña.Count() > 16)
            {
                //Retornamos falso porque no cumple con las condiciones de seguridad
                return false;
            }
            else //Si la contraseña tiene el largo adecuado
            {
                //Declaramos los patrones:
                Regex patronNumerico = new Regex("[0-9]");//si esta del cero al 9
                Regex patronAlfabeticoMay = new Regex("[A-Z]");//si esta de la a a la z mayusculas
                Regex patronAlfabeticoMin = new Regex("[a-z]");//si esta de la a a la z minusculas
                Regex patronSimbolos = new Regex("[^A-Za-z0-9]");// si NO(^) esta ni numeros ni letras... Es decir simbolos y caracteres especiales 

                //Escaneamos la contraseña y vamos guardando lo que obtenemos
                bool hayNumeros = patronNumerico.IsMatch(unaContraseña);
                bool hayMayusculas = patronAlfabeticoMay.IsMatch(unaContraseña);
                bool hayMinusculas = patronAlfabeticoMin.IsMatch(unaContraseña);
                bool haySimbolos = patronSimbolos.IsMatch(unaContraseña);

                //Devolvemos el resultado de escanear la contraseña, si es segura devuelve true:
                return (hayNumeros && hayMayusculas && hayMinusculas && haySimbolos && !unaContraseña.Contains(" "));
            }
            
        }
    }
}
