<?xml version="1.0" encoding="UTF-8"?>

<!-- New document created with EditiX at Wed May 26 23:17:43 CEST 2010 -->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<body>
				<xsl:apply-templates select="catalog/year">
					<xsl:sort select="@y"/>
				</xsl:apply-templates>
			</body>
		</html>
	</xsl:template>
	<xsl:template match="year">
		<h2>
			<xsl:value-of select="@y"/>
		</h2>
		<ul>
			<xsl:apply-templates select="film">
				<xsl:sort select="@author"/>
				<xsl:sort select="@number"/>
			</xsl:apply-templates>
		</ul>
	</xsl:template>
	<xsl:template match="film">
		<li>
			<xsl:value-of select="@author"/>
: 			<xsl:value-of select="@number"/>
		</li>
	</xsl:template>
</xsl:stylesheet>
