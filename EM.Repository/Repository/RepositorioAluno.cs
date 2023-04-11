using EM.Domain.Model;
using FirebirdSql.Data.FirebirdClient;
using System.Linq.Expressions;

namespace EM.Repository.Repository
{
    public class RepositorioAluno : RepositorioAbstrato<Aluno>
    {
        readonly string connectionString = @"User=SYSDBA;Password=masterkey;Database=C:\\Users\\Escolar Manager\\source\\repos\\WebApp\DBALUNOS.fdb;DataSource=localhost;Port=3054;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;";
        public override void Add(Aluno item)
        {
            using (FbConnection connection = new(connectionString))
            {
                string add = "INSERT INTO TBALUNO (ALUMATRICULA, ALUNOME, ALUCPF, ALUNASCIMENTO, ALUSEXO) VALUES((GEN_ID(GEN_TBALUNO, 1)), @nome, @cpf, @nascimento, @sexo);";
                FbCommand command = new(add, connection);

                command.Parameters.Add("@nome", item.Nome.ToLower());
                command.Parameters.Add("@cpf", item.CPF);
                command.Parameters.Add("@nascimento", item.Nascimento.ToString("yyyy/MM/dd"));
                command.Parameters.Add("@sexo", item.Sexo);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public override void Update(Aluno item)
        {
            using (FbConnection connection = new(connectionString))
            {
                string update = "UPDATE TBALUNO SET ALUNOME = @nome, ALUCPF = @cpf, ALUNASCIMENTO = @nascimento, ALUSEXO = @sexo  WHERE ALUMATRICULA = @matricula";
                FbCommand command = new(update, connection);

                command.Parameters.Add("@nome", item.Nome.ToLower());
                command.Parameters.Add("@cpf", item.CPF);
                command.Parameters.Add("@nascimento", item.Nascimento.ToString("yyyy/MM/dd"));
                command.Parameters.Add("@sexo", item.Sexo);
                command.Parameters.Add("@matricula", item.Matricula);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public override void Remove(Aluno item)
        {
            using (FbConnection connection = new(connectionString))
            {
                string remove = $"DELETE FROM TBALUNO WHERE ALUMATRICULA = {item.Matricula}";
                FbCommand command = new(remove, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public override IEnumerable<Aluno> GetAll()
        {
            using (FbConnection connection = new(connectionString))
            {
                string mostraTodosAlunos = "SELECT * FROM TBALUNO ORDER BY ALUMATRICULA;";
                var alunos = new List<Aluno>();
                FbCommand command = new(mostraTodosAlunos, connection);

                try
                {
                    connection.Open();

                    using (FbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Aluno aluno = new()
                            {
                                Matricula = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                CPF = reader.GetString(2),
                                Nascimento = reader.GetDateTime(3),
                                Sexo = (EnumeradorSexo)reader.GetInt32(4)
                            };
                            alunos.Add(aluno);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex) { throw ex; }
                return alunos;
            }
        }

        public Aluno GetByMatricula(int matricula)
        {
            using (FbConnection connection = new(connectionString))
            {
                string alunoPorMatricula = $"SELECT * FROM TBALUNO WHERE ALUMATRICULA = {matricula};";
                FbCommand command = new(alunoPorMatricula, connection);
                Aluno alunos = new();
                try
                {
                    connection.Open();

                    FbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Aluno aluno = new()
                        {
                            Matricula = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            CPF = reader.GetString(2),
                            Nascimento = reader.GetDateTime(3),
                            Sexo = (EnumeradorSexo)reader.GetInt32(4)
                        };
                        alunos = aluno;
                    }
                    connection.Close();
                }
                catch (Exception ex) { throw ex; }
                return alunos;
            }
        }

        public IEnumerable<Aluno> GetByContendoNoNome(string parteDoNome)
        {
            using (FbConnection connection = new(connectionString))
            {
                string alunoPorNome = $"SELECT * FROM TBALUNO WHERE ALUNOME LIKE '%{parteDoNome.ToLower()}%';";
                FbCommand command = new(alunoPorNome, connection);
                var alunosParteDoNome = new List<Aluno>();

                try
                {
                    connection.Open();
                    FbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Aluno aluno = new()
                        {
                            Matricula = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            CPF = reader.GetString(2),
                            Nascimento = reader.GetDateTime(3),
                            Sexo = (EnumeradorSexo)reader.GetInt32(4)
                        };
                        alunosParteDoNome.Add(aluno);
                    }
                    connection.Close();
                }
                catch (Exception ex) { throw ex; }
                return alunosParteDoNome;
            }
        }

        public override IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            IEnumerable<Aluno> alunos= GetAll();
            
                return alunos.Where(predicate.Compile());
        }
    }
}
