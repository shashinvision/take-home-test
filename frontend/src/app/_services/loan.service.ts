import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Environment } from "../_models/environment.model";
import { Loan } from "../_models/loan";
import { Applicants } from "../_models/applicants";
import { loanPayload } from "../_models/loanPayload";

@Injectable({
  providedIn: "root",
})
export class LoanService {
  private apiUrl: string = (environment as Environment).API_URL;

  constructor(private http: HttpClient) {}

  getLoans() {
    return this.http.get<Loan[]>(`${this.apiUrl}/loans`);
  }

  getApplicants() {
    return this.http.get<Applicants[]>(`${this.apiUrl}/applicants`);
  }

  getLoanById(id: number) {
    return this.http.get<Loan>(`${this.apiUrl}/loans/${id}`);
  }

  createLoan(payload: loanPayload) {
    return this.http.post(`${this.apiUrl}/loans`, payload);
  }
}
