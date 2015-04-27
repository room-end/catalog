<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="*">
    <xsl:copy>
      <xsl:apply-templates select="@number">
        <xsl:sort select="name()" />
      </xsl:apply-templates>
      <xsl:apply-templates select="@author">
        <xsl:sort select="name()" />
      </xsl:apply-templates>
      <xsl:apply-templates select="@type">
        <xsl:sort select="name()" />
      </xsl:apply-templates>
      <xsl:apply-templates select="@xumber">
        <xsl:sort select="name()" />
      </xsl:apply-templates>      
      <xsl:apply-templates select="@state">
        <xsl:sort select="name()" />
      </xsl:apply-templates>      
      <xsl:apply-templates select="@y">
        <xsl:sort select="name()" />
      </xsl:apply-templates>
      <xsl:apply-templates/>
    </xsl:copy>
  </xsl:template>
  <xsl:template match="@*|comment()|processing-instruction()">
    <xsl:copy />     
  </xsl:template>
</xsl:stylesheet>