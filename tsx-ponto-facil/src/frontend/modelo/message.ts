const message = {
    serverError: `Erro no servidor`,
    genericError: `Erro`,
    genericSuccess: `Sucesso`,
    employee: {
        successCreated: `Funcionário cadastrado com successo!`,
        successUpdated: `Funcionário atualizado com sucesso!`,
        successRemoved: `Funcionário excluído com sucesso!`,
        ensureRemove: `Deseja excluir o funcionário?`
    },
    presence: {
        successRegister: `Presença registrada com sucesso!`,
        successActivated: `Presença vistada com sucesso!`,
        ensureActivation: `Deseja marcar a presença com visto?`,
        infoWorktime: `Para efeitos de cálculo, trabalhos não finalizados contam como no máximo 12 horas incluídas antes do fim do período pesquisado, e trabalhos finalizados sem início consideram seu início há pelo menos 12 horas depois do início do período pesquisado`
    },
    place: {
        loadingError: `Erro ao carregar Local do Dispositivo.`,
        loadingErrorVerifyPermission: `Erro ao carregar Local do Dispositivo: verifique se você está acessando o site via HTTPS e se o site tem permissão para obter a localização.`
    }
}

export { message }