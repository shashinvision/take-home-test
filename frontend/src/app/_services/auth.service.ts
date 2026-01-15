import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, tap } from "rxjs";
import { Router } from "@angular/router";

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
  private readonly API_URL = "http://localhost:3000/api"; // Cambiá esto por tu API

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.API_URL}/auth/login`, {
        email,
        password,
      })
      .pipe(
        tap((response) => {
          // Guardamos el token en localStorage
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
