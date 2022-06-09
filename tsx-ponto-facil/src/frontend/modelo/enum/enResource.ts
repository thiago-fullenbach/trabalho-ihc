export default class EnResource {

    resourceCode: number

    constructor(resourceCode: number) {
        this.resourceCode = resourceCode
    }
    static get VisualizarUsuario(): EnResource {
        return new EnResource(1)
    }
    static get CadastrarUsuario(): EnResource {
        return new EnResource(2)
    }
    static get VisualizarDemaisUsuarios(): EnResource {
        return new EnResource(3)
    }
    static get CadastrarDemaisUsuarios(): EnResource {
        return new EnResource(4)
    }
    static get CadastrarAcessoTodosUsuarios(): EnResource {
        return new EnResource(5)
    }
    static get VisualizarPonto(): EnResource {
        return new EnResource(6)
    }
    static get RegistrarPonto(): EnResource {
        return new EnResource(7)
    }
    static get VisualizarPontoDemaisUsuarios(): EnResource {
        return new EnResource(8)
    }
    static get VisualizarAjuste(): EnResource {
        return new EnResource(9)
    }
    static get VisualizarAjusteDemaisUsuarios(): EnResource {
        return new EnResource(10)
    }
    static get RegistrarAjusteDemaisUsuarios(): EnResource {
        return new EnResource(11)
    }
    equals = (outer: EnResource) => this.resourceCode === outer.resourceCode

}