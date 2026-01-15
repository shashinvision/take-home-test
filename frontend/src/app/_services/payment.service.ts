import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Environment } from "../_models/environment.model";
import { paymentPayload } from "../_models/paymentPayload";

@Injectable({
  providedIn: "root",
})
export class PaymentService {
  private apiUrl: string = (environment as Environment).API_URL;

  constructor(private http: HttpClient) {}

  createLoan(payload: paymentPayload) {
    return this.http.post(`${this.apiUrl}/payment`, payload);
  }
}
