import axios from "axios"
import React, { useEffect } from "react"
import ApiUtil from "../../../../../../chamada-api/ApiUtil"
import { toDisplayedHour, toDisplayedValue, mountMessageServerErrorIfDefault, toDisplayedValueLocal, toDisplayedHourLocal, formatCep, formatDate_hh_mm } from "../../../../../../utils/formatting"
import { isFieldValid } from "../../../../../../utils/valid"
import SessionState from "../../../../../main/SessionState"
import { message } from "../../../../../modelo/message"
import PlaceAdapter from "../../../../../modelo/place/Adapter/placeAdapter"
import Place from "../../../../../modelo/place/place"
import NewPresence from "../../../../../modelo/presence/newPresence"
import LoadingState from "../../../../templates/LoadingModal/LoadingState"
import EnSelectTipoPresenca from "./Enum/EnSelectTipoPresenca"

type NewPresenceCrudProps = {
    updateInputField: (event: React.ChangeEvent<HTMLInputElement>) => void
    updatePresenceTypeFromServer: (eh_entrada: boolean) => void
    updatePlace: (place: Place) => void
    publicValidateNewPresence: () => void
    publicCancelForm: () => void
    novaPresencaEhEntrada: boolean | null
    newPresence: NewPresence
    sessionState: SessionState
    loadingState: LoadingState
    informaErroGenerico: (mensagem: string) => void
    updateApiReqCompleted: (apiReqCompleted: boolean) => void
    updateOpenStreetMapReqCompleted: (openStreetMapReqCompleted: boolean) => void
}

export default (props: NewPresenceCrudProps): JSX.Element => {
    const [currentDatetime, setCurrentDatetime] = React.useState(new Date())

    useEffect(() => {
        (async (): Promise<void> => {
            props.updateApiReqCompleted(false);
            let r = await ApiUtil.getAsync<boolean>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Presenca/completarNovaPresencaEhEntrada`)
            let eh_entradaServer = r.body?.getReturned();
            let eh_entradaTrueOrFalse = false;
            if (eh_entradaServer == true)
            {
                eh_entradaTrueOrFalse = true;
            }
            if (r.status === 200) {
                props.updatePresenceTypeFromServer(eh_entradaTrueOrFalse)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body);
                props.updateApiReqCompleted(true);
                props.informaErroGenerico(mensagemCompleta)
            }
        })()
        loadDevicePlace()
    }, [])

    useEffect(() => {
        setCurrentDatetime(new Date());
    }, [new Date().getMinutes()])

    const isPlaceLoaded = (place: Place) => {
        return (
            isFieldValid(place.cep)
            && isFieldValid(place.nome_logradouro)
            && isFieldValid(place.nome_bairro)
            && isFieldValid(place.nome_cidade)
            && isFieldValid(place.nome_estado)
        )
    }

    const loadDevicePlace = () => {
        props.updateOpenStreetMapReqCompleted(false);
        navigator.geolocation.getCurrentPosition(location => {
            const storeCoords = location.coords
            axios.get(`http://nominatim.openstreetmap.org/reverse?format=json&lat=${storeCoords.latitude}&lon=${storeCoords.longitude}&zoom=18&addressdetails=1`)
            .then(response => {
                let loadedPlace: Place = new PlaceAdapter(response.data)
                props.updatePlace(loadedPlace)
            })
            .catch(response => {
                props.informaErroGenerico(message.place.loadingError)
                props.updateOpenStreetMapReqCompleted(true)
            })
        }, () => {
            props.informaErroGenerico(message.place.loadingErrorVerifyPermission)
            props.updateOpenStreetMapReqCompleted(true)
        })
    }

    return (
        <div className="form">
            <div className="row">
                <div className="col-12">
                    <div className="form-group">
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label><strong>Nova Presença</strong></label>
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Funcionário</label>
                                <input type="text" 
                                    className="form-control"
                                    value={props.sessionState.loggedUser?.Nome.split(' ')[0]}
                                    disabled />
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Tipo da Presença</label>
                                <select
                                    className="form-control" 
                                    name="select_tipo_presenca"
                                    disabled value={props.novaPresencaEhEntrada == null ? EnSelectTipoPresenca.Selecione.enValue :
                                        (props.novaPresencaEhEntrada ? EnSelectTipoPresenca.InicioTrabalho.enValue : EnSelectTipoPresenca.FimTrabalho.enValue)} >
                                    <option value={EnSelectTipoPresenca.Selecione.enValue} >{EnSelectTipoPresenca.Selecione.enDesc}</option>
                                    <option value={EnSelectTipoPresenca.InicioTrabalho.enValue} >{EnSelectTipoPresenca.InicioTrabalho.enDesc}</option>
                                    <option value={EnSelectTipoPresenca.FimTrabalho.enValue} >{EnSelectTipoPresenca.FimTrabalho.enDesc}</option>
                                </select>
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Data Atual</label>
                                <input type="date"
                                    className="form-control" 
                                    name="data_nascimento"
                                    value={toDisplayedValueLocal(currentDatetime)}
                                    disabled />
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Hora Atual</label>
                                <input type="hour"
                                    className="form-control" 
                                    name="data_nascimento"
                                    value={toDisplayedHourLocal(currentDatetime)}
                                    disabled />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <hr />
            <div className="row">
                <div className="col-12">
                    <div className="form-group">
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label><strong>Local</strong></label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            {!isPlaceLoaded(props.newPresence.local) && <hr />}
            {!isPlaceLoaded(props.newPresence.local) && <div className="row">
                <div className="col-12 d-flex justify-content-end">
                    <button className="btn btn-primary"
                        onClick={_ => loadDevicePlace()}>
                        Carregar Local do Dispositivo
                    </button>
                </div>
            </div>}
            <hr />
            <div className="row">
                <div className="col-12">
                    <div className="form-group">
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>CEP</label>
                                <input type="text"
                                    className="form-control"
                                    value={formatCep(props.newPresence.local.cep)}
                                    disabled />
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Rua</label>
                                <input type="text"
                                    className="form-control" 
                                    value={props.newPresence.local.nome_logradouro}
                                    disabled />
                            </div>
                        </div>
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Número</label>
                                <input type="text"
                                    className="form-control"
                                    name="numero_local"
                                    value={props.newPresence.local.numero_logradouro}
                                    onChange={e => props.updateInputField(e)} />
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Complemento</label>
                                <input type="text"
                                    className="form-control" 
                                    name="complemento_local"
                                    value={props.newPresence.local.complemento_logradouro}
                                    onChange={e => props.updateInputField(e)} />
                            </div>
                        </div>
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Bairro</label>
                                <input type="text"
                                    className="form-control"
                                    value={props.newPresence.local.nome_bairro}
                                    disabled />
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Cidade</label>
                                <input type="text"
                                    className="form-control" 
                                    value={props.newPresence.local.nome_cidade}
                                    disabled />
                            </div>
                        </div>
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Estado</label>
                                <input type="text"
                                    className="form-control"
                                    value={props.newPresence.local.nome_estado}
                                    disabled />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div className="row">
                <div className="col-12 d-flex justify-content-end">
                    <button className="btn btn-primary"
                        onClick={_ => props.publicValidateNewPresence()}>
                        Salvar
                    </button>

                    <button className="btn btn-secondary ms-2"
                        onClick={_ => props.publicCancelForm()}>
                        Cancelar
                    </button>
                </div>
            </div>

            
        </div>
    )
}