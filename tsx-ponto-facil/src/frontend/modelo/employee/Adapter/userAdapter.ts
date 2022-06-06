import DetailedUserDTO from "../DTO/detailedUserDTO";
import User from "../user";

export default class UserAdapter {
    
    detailedUserAdaptee?: DetailedUserDTO

    constructor(detailedUser: DetailedUserDTO) {
        this.detailedUserAdaptee = detailedUser
    }
    getRawUser(): User {
        let rawUser = new User()
        rawUser.id = this.detailedUserAdaptee?.id || 0
        rawUser.nome = this.detailedUserAdaptee?.nome || ``
        rawUser.cpf = this.detailedUserAdaptee?.cpf || ``
        rawUser.data_nascimento = this.detailedUserAdaptee?.data_nascimento || new Date()
        rawUser.horas_diarias = this.detailedUserAdaptee?.horas_diarias || 0
        rawUser.login = this.detailedUserAdaptee?.login || ``
        rawUser.acessos = this.detailedUserAdaptee?.acessos || []
        return rawUser
    }
}