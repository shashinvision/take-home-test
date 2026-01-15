import { Component, inject, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TableComponent } from "../../_components/table/table.component";
import { AuthService } from "../../_services/auth.service";

@Component({
  selector: "app-home",
  standalone: true,
  imports: [CommonModule, TableComponent],
  templateUrl: "./home.component.html",
  styleUrl: "./home.component.scss",
})
export class HomeComponent implements OnInit {
  private authService = inject(AuthService);
  name: string | null = null;

  ngOnInit(): void {
    this.name = this.authService.getClaim("name");
  }

  logout(): void {
    this.authService.logout();
  }
}
