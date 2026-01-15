import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, tap } from "rxjs";
import { Router } from "@angular/router";
import { environment } from "../../environments/environment";
import { Environment } from "../_models/environment.model";

export interface LoginResponse {
  token: string;
  user: {
    id: string;
    email: string;
    name: string;
  };
}

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl: string = (environment as Environment).API_URL;

  login(Email: string, Password: string): Observable<LoginResponse> {
    const url = `${this.apiUrl}/api/auth/login`;
    return this.http
      .post<LoginResponse>(`${url}`, {
        Email: Email,
        Password: Password,
      })
      .pipe(
        tap((response) => {
          // keep token in localStorage
          localStorage.setItem("token", response.token);
          localStorage.setItem("user", JSON.stringify(response.user));
        }),
      );
  }

  logout(): void {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    this.router.navigate(["/login"]);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem("token");
  }

  getToken(): string | null {
    return localStorage.getItem("token");
  }
}
