import { LocationStrategy, PathLocationStrategy } from "@angular/common";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { provideAnimations } from "@angular/platform-browser/animations";
import { API_URL_ROOT } from "@core";
import { apiResponseInterceptor } from "@core/interceptors/api-response-interceptor";
import { tokenInterceptor } from "@core/interceptors/token-interceptor";
import { ConfirmationService, MessageService } from "primeng/api";
import { BlockUIModule } from "primeng/blockui";
import { ToastModule } from "primeng/toast";
import { environment } from "src/environments/environment";
import { AppComponent } from "./app.component";
import { RouterModule } from "@angular/router";
import { RouteConfig, Routes } from "@routing/routes";
import { CreateSwapComponent } from './user/components/pages/create-swap/create-swap.component'; // Import CreateSwapComponent
import { BrowseSwapsComponent } from './user/components/pages/browse-swaps/browse-swaps.component';

@NgModule({
  declarations: [AppComponent],
  imports: [
    RouterModule.forRoot(Routes, RouteConfig),
    BrowserModule, ToastModule, BlockUIModule,
    CreateSwapComponent, BrowseSwapsComponent],
  providers: [
    provideAnimations(),
    { provide: API_URL_ROOT, useValue: environment.apiUrlRoot },
    { provide: LocationStrategy, useClass: PathLocationStrategy },
    provideHttpClient(withInterceptors([tokenInterceptor, apiResponseInterceptor])),
    MessageService,
    ConfirmationService
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
