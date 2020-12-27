using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Modelo.Cadastros
{
    public class Instituicao
    {
        public long? InstituicaoID { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public virtual ICollection<Departamento> Departamentos { get; set; }

        //Definir elementos como virtual possibilita a sua sobrescrita, o que, para o EF Core, é necessário.
        //Assim, ele poderá fazer o LazyLoad(carregamento tardio), por meio de um padrão de projeto conhecido como Proxy.
    }
}
