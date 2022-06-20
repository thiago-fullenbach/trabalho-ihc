import Place from "../place";

export default class PlaceAdapter extends Place {

    constructor(openStreetMapData: any) {
        super()
        this.cep = openStreetMapData.address.postcode;
        this.nome_logradouro = openStreetMapData.address.road;
        this.nome_bairro = openStreetMapData.address.city_district;
        this.nome_cidade = openStreetMapData.address.city;
        this.nome_estado = openStreetMapData.address.state;
    }
}