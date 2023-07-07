# Cloudflare

terraform {
  required_providers {
    cloudflare = {
      source = "cloudflare/cloudflare"
      version = "~> 3.0"
    }
  }
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}

provider "local" {

}

# Cloudflare
resource "cloudflare_record" "cloudflare-a-record-template-fps" {
  zone_id = var.cloudflare_zone_id
  name    = var.deploy_domain
  value   = var.deploy_ip
  type    = "A"
  proxied = true
}

resource "tls_private_key" "cloudflare-origin-private-key" {
  algorithm = "RSA"
}

resource "tls_cert_request" "cloudflare-origin-cert-request" {
  private_key_pem = tls_private_key.cloudflare-origin-private-key.private_key_pem

  subject {
    common_name  = "Template FPS Certificate"
    organization = "Elympics"
  }
}

resource "cloudflare_origin_ca_certificate" "cloudflare-origin-certificate" {
  csr                = tls_cert_request.cloudflare-origin-cert-request.cert_request_pem
  hostnames          = [cloudflare_record.cloudflare-a-record-template-fps.name]
  request_type       = "origin-rsa"
  requested_validity = 1095
}

resource "local_file" "private_key" {
    content  = tls_private_key.cloudflare-origin-private-key.private_key_pem
    filename = "outputs/${var.deploy_domain}.key"
}

resource "local_file" "origin_certificate" {
    content  = cloudflare_origin_ca_certificate.cloudflare-origin-certificate.certificate
    filename = "outputs/${var.deploy_domain}.crt"
}
