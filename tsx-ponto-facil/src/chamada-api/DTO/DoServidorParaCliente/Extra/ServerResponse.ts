import { Axios, AxiosResponse, AxiosResponseHeaders } from "axios";
import IReturnedXMessagesAdapter from "../Adapter/IReturnedXMessagesAdapter";
import LoggedUserDTO from "../LoggedUserDTO";
import ReturnedXMessagesFailureAdapter from "../ReturnedXMessagesImpl/Adapter/ReturnedXMessagesFailureAdapter";
import ReturnedXMessagesSuccessAdapter from "../ReturnedXMessagesImpl/Adapter/ReturnedXMessagesSuccessAdapter";
import ReturnedXMessagesFailure from "../ReturnedXMessagesImpl/ReturnedXMessagesFailure";
import ReturnedXMessagesSuccess from "../ReturnedXMessagesImpl/ReturnedXMessagesSuccess";

export default class ServerResponse<RType> {
    status = 0
    stringifiedSession = ``
    loggedUser?: LoggedUserDTO
    body?: IReturnedXMessagesAdapter<RType>

    hasSuccess(response: AxiosResponse<ReturnedXMessagesSuccess<RType>, any>): ServerResponse<RType> {
      this.status = response.status;
      this.stringifiedSession = response.headers["sessao"];
      let loggedUserSession = JSON.parse(response.headers["usuario"]);
      this.loggedUser = loggedUserSession;
      this.body = new ReturnedXMessagesSuccessAdapter(response.data)
      return this;
    }
    hasFailure(response: AxiosResponse<ReturnedXMessagesFailure<RType>, any>): ServerResponse<RType> {
      this.status = response.status;
      this.body = new ReturnedXMessagesFailureAdapter(response.data)
      return this;
    }
}
