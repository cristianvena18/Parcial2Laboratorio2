using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace ParcialLab2._2
{
    class Program
    {
        static Hashtable table = new Hashtable();

        static void Main(string[] args)
        {
            do
            {

                Console.Clear();
                Console.WriteLine("********************************");
                Console.WriteLine("****** PRODUCTOS EMPRESA *******");
                Console.WriteLine("********************************");
                Console.WriteLine("*      1- Agregar producto     *");
                Console.WriteLine("*      2- Modificar producto   *");
                Console.WriteLine("*      3- Eliminar producto    *");
                Console.WriteLine("*      4- Listar productos     *");
                Console.WriteLine("*      5- Buscar producto      *");
                Console.WriteLine("*      6- Importar             *");
                Console.WriteLine("*      7- Salir                *");
                Console.WriteLine("*******************************");

                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    if (result == 7)
                    {
                        break;
                    }

                    try
                    {
                        Menu(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: {0}", e.Message);
                        Console.WriteLine("Presione una tecla para continuar...");
                        Console.ReadKey();
                    }
                }

            } while (true);
            MostrarSaludo();
        }

        public static void MostrarSaludo()
        {
            char[] msj = "Muchas gracias por utilizar el sistema!".ToCharArray();

            foreach (var item in msj)
            {
                Console.Write(item);
                if (item == ' ')
                {
                    Thread.Sleep(60);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            Thread.Sleep(600);
        }
        //METODO LISTAR
        static void Listar(Hashtable productos)
        {
            if (productos.Count == 0)
            {
                throw new Exception("No existen productos cargados!");
            }

            do
            {
                Console.WriteLine("Como desea listar los productos? 1-NO ordenados / 2 - Ordenados");
                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    if (opcion == 1)
                    {
                        //Listar todos
                        Console.WriteLine("** PRODUCTOS ***");
                        Console.WriteLine("****************");
                        ListarTodos(productos);
                        break;
                    }
                    else if (opcion == 2)
                    {
                        //Listar todos ordenados
                        Console.WriteLine("* PRODUCTOS ORDENADOS *");
                        Console.WriteLine("***********************");
                        ListarTodosOrdenados(productos);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ingrese una opcion valida");
                    }
                }
                else
                {
                    Console.WriteLine("Ingrese un numero valido!");
                }

            } while (true);
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        //METODO LISTAR TODOS
        static void ListarTodos(Hashtable productos)
        {
            foreach (DictionaryEntry producto in productos)
            {
                Console.WriteLine("[{0}] [{1}]", producto.Key, producto.Value);
            }
        }

        //METODO LISTAR TODOS ORDENADOS
        static void ListarTodosOrdenados(Hashtable productos)
        {
            var ordenarKey = productos.Keys.Cast<string>().OrderBy(c => c);
            ListarTodos(productos);
        }

        /// <summary>
        /// METODO BUSCAR DESCRIPCION
        /// </summary>
        /// <param name="productos"></param>
        static void Buscar(Hashtable productos)
        {
            if (productos.Count == 0)
            {
                throw new Exception("No existe ningun producto cargado !");
            }


            string descripcion;
            do
            {
                Console.WriteLine("");
                Console.WriteLine("Ingrese descripcion para filtrar productos:");
                descripcion = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(descripcion))
                {
                    Console.WriteLine("Ingrese una descripcion valida !");
                }
                else
                {
                    break;
                }
            } while (true);

            Console.WriteLine("** PRODUCTOS ***");
            Console.WriteLine("****************");
            int cant = 0;
            foreach (DictionaryEntry producto in productos)
            {
                if (producto.Value.ToString().StartsWith(descripcion))
                {
                    Console.WriteLine("[{0}] [{1}]", producto.Key, producto.Value);
                    cant++;
                }
            }
            if (cant == 0)
            {
                Console.WriteLine("No existe ningun producto con esa descripcion !");
            }

            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        private static void Menu(int result)
        {
            Console.Clear();
            switch (result)
            {
                case 1:
                    CargarProducto(Program.table);
                    break;
                case 2:
                    ModificarProducto(Program.table);
                    break;
                case 3:
                    EliminarProducto(Program.table);
                    break;
                case 4:
                    Listar(Program.table);
                    break;
                case 5:
                    Buscar(Program.table);
                    break;
                case 6:
                    ImportarProductos();
                    break;
                default:
                    break;
            }
        }

        private static void ImportarProductos()
        {
            StringCollection codigos = new StringCollection()
            {
                "abcd12345", "abcd11111", "abcd11112", "abcdefg12", "abcddsrtu"
            };
            StringCollection descriptions = new StringCollection()
            {
                "Este es el primer producto", "Este es el segundo producto",
                "Este es el tercer producto", "Este es el cuarto producto",
                "Este es el quinto producto"
            };

            do
            {
                Console.WriteLine("¿Seguro de importar? s/n");
                string entrada = Console.ReadLine();
                if (entrada.ToLower() == "s")
                {
                    Program.table.Clear();
                    break;
                }
                else if (entrada.ToLower() == "n")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Ingrese las opciones!");
                }

            } while (true);

            for (int i = 0; i < codigos.Count; i++)
            {
                CargarProducto(codigos[i], descriptions[i]);
            }
        }

        private static void EliminarProducto(Hashtable table)
        {
            if (table.Count == 0)
            {
                throw new ArgumentNullException("table", "No hay productos cargados!");
            }

            string code;
            do
            {
                Console.Write("Ingrese un código: ");

                code = Console.ReadLine();

                if (IsCodeValid(code))
                {
                    if (!VerificarCodigoNoRepetido(table, code))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("El código no existe en el sistema! Ingrese otro");
                    }
                }
                else
                {
                    Console.WriteLine("El codigo no tiene el formato valido!");
                }

            } while (true);

            do
            {
                Console.WriteLine("¿Quiere proseguir? s/n");
                string entrada = Console.ReadLine();
                if (entrada.ToLower() == "s")
                {
                    table.Remove(code);
                    break;
                }
                else if (entrada.ToLower() == "n")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Ingrese las opciones!");
                }

            } while (true);

            Program.table = table;
        }

        private static void ModificarProducto(Hashtable table)
        {
            if (table.Count == 0)
            {
                throw new ArgumentNullException("table", "No hay productos cargados!");
            }

            string code;
            do
            {
                Console.Write("Ingrese un código: ");

                code = Console.ReadLine();

                if (IsCodeValid(code))
                {
                    if (!VerificarCodigoNoRepetido(table, code))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("El código no existe en el sistema! Ingrese otro");
                    }
                }
                else
                {
                    Console.WriteLine("El codigo no tiene el formato valido!");
                }

            } while (true);

            do
            {
                Console.Write("Ingrese una descripcion: ");
                string description = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("Error, debe proporcionar una descripcion");
                }
                else
                {
                    table[code] = description;
                    break;
                }

            } while (true);

            Program.table = table;
        }

        private static void CargarProducto(Hashtable table)
        {
            string code;
            do
            {
                Console.Write("Ingrese un código de producto: ");

                code = Console.ReadLine();

                try
                {
                    if (IsCodeValid(code))
                    {
                        if (VerificarCodigoNoRepetido(table, code))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("El código ya esta cargado! Ingrese otro");
                        }
                    }
                    else
                    {
                        Console.WriteLine("El codigo no tiene el formato valido! Ingrese otro");
                    }
                }
                catch(FormatException)
                {
                    break;
                }
                catch (Exception)
                {
                    throw;
                }
            } while (true);

            do
            {
                Console.Write("Ingrese una descripcion de producto: ");
                string description = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("Error, debe proporcionar una descripcion");
                }
                else
                {
                    CargarProducto(code, description);
                    break;
                }

            } while (true);
        }

        private static void CargarProducto(string code, string description)
        {
            if (code.Length != 9)
            {
                throw new FormatException("El codigo es erroneo, error cargando un producto");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new FormatException("El codigo es erroneo, error cargando un producto");
            }

            Program.table.Add(code, description);
        }

        private static bool VerificarCodigoNoRepetido(Hashtable table, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new Exception("El codigo no puede ser nulo!");
            }

            if (table.Count == 0)
            {
                throw new FormatException();
            }

            return !table.ContainsKey(code);
        }

        public static bool IsCodeValid(string code)
        {
            if (code.Length != 9)
            {
                throw new Exception("El codigo debe tener 9 digitos");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            char[] vs = code.ToCharArray();

            int cnt = (from item in vs
                       where char.IsLetterOrDigit(item) 
                       select item).Count();

            return cnt == code.Length;
        }
    }
}
