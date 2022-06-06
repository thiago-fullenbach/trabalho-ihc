import User from "../../user";
import EditUserDTO from "../editUserDTO";

export default class EditUserDTOAdapter {
    
    userAdaptee?: User

    constructor(user: User) {
        this.userAdaptee = user
    }
    getRawEditUserDTO(): EditUserDTO {
        let rawEditUser = new EditUserDTO()
        rawEditUser.id = this.userAdaptee?.id || 0
        rawEditUser.nome = this.userAdaptee?.nome || ``
        rawEditUser.cpf = this.userAdaptee?.cpf || ``
        rawEditUser.data_nascimento = this.userAdaptee?.data_nascimento || new Date()
        rawEditUser.horas_diarias = this.userAdaptee?.horas_diarias || 0
        rawEditUser.login = this.userAdaptee?.login || ``
        rawEditUser.nova_senha = this.userAdaptee?.nova_senha || null
        rawEditUser.acessos = this.userAdaptee?.acessos || []
        return rawEditUser
    }
}