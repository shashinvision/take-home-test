import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Environment } from "../_models/environment.model";
import { Loan } from "../_models/loan";

@Injectable({
  providedIn: "root",
})
export class LoanService {
  private apiUrl: string = (environment as Environment).API_URL;

  constructor(private http: HttpClient) {}

  getLoans() {
    return this.http.get<Loan[]>(`${this.apiUrl}/loans`);
  }
}
