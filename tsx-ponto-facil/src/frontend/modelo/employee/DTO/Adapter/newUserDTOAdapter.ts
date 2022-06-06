import User from "../../user";
import NewUserDTO from "../newUserDTO";

export default class NewUserDTOAdapter {
    
    userAdaptee?: User

    constructor(user: User) {
        this.userAdaptee = user
    }
    getRawNewUserDTO(): NewUserDTO {
        let rawNewUser = new NewUserDTO()
        rawNewUser.nome = this.userAdaptee?.nome || ``
        rawNewUser.cpf = this.userAdaptee?.cpf || ``
        rawNewUser.data_nascimento = this.userAdaptee?.data_nascimento || new Date()
        rawNewUser.horas_diarias = this.userAdaptee?.horas_diarias || 0
        rawNewUser.login = this.userAdaptee?.login || ``
        rawNewUser.nova_senha = this.userAdaptee?.nova_senha || ``
        rawNewUser.acessos = this.userAdaptee?.acessos || []
        return rawNewUser
    }
}