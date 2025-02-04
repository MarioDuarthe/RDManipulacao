namespace Domain.Entities
{
    public class UsuarioModel
    {
        public string nome { get; private set; }
        public string usuario { get; private set; }
        public string senha { get; private set; }

        public UsuarioModel(){  }

        public UsuarioModel(string nome, string usuario, string senha)
        {
            this.nome = nome;
            this.usuario = usuario;
            this.senha = senha;
        }
    }
}
