class History {
    private path: string = "/"; // Başlangıç yolu
  
    // Geçerli yolu al
    getCurrentPath(): string {
      return this.path;
    }
  
    // Sayfa yönlendirme
    push(path: string): void {
      this.path = path;
      window.history.pushState({}, '', path); // URL'yi güncelle
      window.dispatchEvent(new Event('popstate')); // Sayfa değişikliğini tetikle
      // window.location.reload(); // Sayfayı yeniden yükle
      //deneme
    }
  
    // Sayfa geri gitme
    goBack(): void {
      window.history.back();
    }
  }
  
  const history = new History();
  export default history;
  