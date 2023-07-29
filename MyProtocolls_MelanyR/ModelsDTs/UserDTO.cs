namespace MyProtocolls_MelanyR.ModelsDTs
{
    public class UserDTO
    {
        //a modo de ejemplo los atributos de la clase estaran en español

        public int UsuarioId { get; set; }
        public string Correo { get; set; } = null!;
        public string Contrasenia { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string RespaldoCorreo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string? Direccion { get; set; }
        public bool? Activo { get; set; }
        public bool? EstaBloqueado { get; set; }
        public int IDRolUsuario { get; set; }
        public string DescripcionRol { get; set; } = null!;

        //funciones
         public async Task<UserDTO> GetUserInfo(string PEmail)
        {

        }
    }
}
