namespace MyProtocolls_MelanyR.ModelsDTs
{
    public class UserRoleDTO
    {
        //un DTO (data transform object) sirbe para dos funciones:
        //1-simpleficar la estructura de los json que se envian y llegan a los end point de los 
        //controllers quitando composiciones innesesarias que solo harian que los json sean muy pesados
        //o que muestren informacion que no se decea ver
        //2- Ocultar la estructura real de los modelos y por tanto de la tabola de la base de datos

        //tomando en cuanta el segundo criterio, y solo a manera de ejemplo este DTO tendra los nombres 
        //de las propiedades en español
        public int IDRolUsuario { get; set; }
        public string DescripcionRol { get; set; } = null!;
    }
}
